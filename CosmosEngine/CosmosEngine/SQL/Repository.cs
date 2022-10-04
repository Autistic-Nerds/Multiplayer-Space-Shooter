using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cosmos.SQLite
{
	public class Repository<T> : Repository, IRepository<T>, IDisposable where T : IRepositoryElement, new()
	{
		#region Fields
		private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private string name;
		private readonly IDatabaseProvider provider;
		private readonly IMapper<T> mapper;
		private IDbConnection? connection;

		/// <summary>
		/// 
		/// </summary>
		private readonly string dataValues;
		/// <summary>
		/// The keys (coloums) for the <see cref="Cosmos.SQLite.Repository{T}{T}"/>
		/// </summary>
		private readonly string[] dataKeys;
		private int count;
		private int capacity;
		private bool disposed;

		/// <summary>
		/// The name of the <see cref="Cosmos.SQLite.Repository{T}{T}"/>.
		/// </summary>
		public string Name { get => name; set => Rename(value); }
		/// <summary>
		/// The amount of elements <see cref="Cosmos.SQLite.Repository{T}{T}"/>.
		/// </summary>
		public int Count => count;
		/// <summary>
		/// The maximum amount of elements that can exist in the <see cref="Cosmos.SQLite.Repository{T}{T}"/>, default is limited to <see cref="Int32.MaxValue"/>, setting the limit lower to the amount of count in the data set will remove excess elements.
		/// </summary>
		public int Capacity 
		{ 
			get => capacity;
			set
			{
				capacity = value;
				InsureCapacity();
			}
		}
		/// <summary>
		/// The readable version of the keys (coloums) for the <see cref="Cosmos.SQLite.Repository{T}{T}"/>.
		/// </summary>
		public string[] Keys => dataKeys;
		/// <summary>
		/// Gets or sets the element at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[int index] { get => Get(index); set => Replace(index, value); }

		#endregion

		public Repository(string name, IDatabaseProvider provider, IMapper<T> mapper) : this(name, provider, mapper, int.MaxValue) { }
		public Repository(string name, IDatabaseProvider provider, IMapper<T> mapper, int capacity)
		{
			this.name = name.Replace(' ', '_');
			this.provider = provider;
			this.mapper = mapper;
			this.capacity = capacity;
			this.dataValues = BuildElementKeys();

			string dataStructure = "ID," + dataValues.Remove(dataValues.Length - 1, 1).Remove(0, 1);
			string[] keys = dataStructure.Split(",");
			this.dataKeys = keys;
		}

		~Repository() => Dispose(false);

		#region SQL Commands

		private string BuildElementKeys()
		{
			int dataCount = 0;
			StringBuilder sb = new StringBuilder();
			FieldInfo[] fieldInfos = typeof(T).GetFields(bindingFlags);
			if (fieldInfos.Length > 0)
			{
				sb.Append("(");
				for (int i = 0; i < fieldInfos.Length; i++)
				{
					FieldInfo field = fieldInfos[i];
					if (SerialisedValue.IsSerialisableField<T>(field, out SerialisedValue value))
					{
						if(dataCount > 0)
							sb.Append(",");
						if (value == null || string.IsNullOrEmpty(value.Identifier))
							sb.Append(field.Name);
						else
							sb.Append(value.Identifier);
						dataCount++;
					}
				}
				sb.Append(")");
			}
			return sb.ToString();
		}

		private string BuildElementValues(T element)
		{
			int dataCount = 0;
			StringBuilder sb = new StringBuilder();
			FieldInfo[] fieldInfos = typeof(T).GetFields(bindingFlags);
			if (fieldInfos.Length > 0)
			{
				sb.Append("(");
				for (int i = 0; i < fieldInfos.Length; i++)
				{
					FieldInfo field = fieldInfos[i];
					if (SerialisedValue.IsSerialisableField<T>(field))
					{
						object? value = element != null ? field.GetValue(element) : null;
						if (dataCount > 0)
							sb.Append(", ");

						if (value == null)
						{
							sb.Append("''");
						}
						else
						{
							var converter = TypeDescriptor.GetConverter(field.FieldType);
							if (converter == null)
								throw new NullReferenceException($"No converter for the type {field.FieldType} exist - this value type cannot be used for the repository");

							sb.Append($"'{converter.ConvertToString(value)}'");
						}
						dataCount++;
					}
				}
				sb.Append(")");
			}
			return sb.ToString();
		}
		/// <summary>
		/// Returns true if <paramref name="variable"/> is an associated key assigned to the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="variable">The name of the key to check.</param>
		internal bool IsValidKey(string variable)
		{
			bool valid = false;
			foreach (string key in Keys)
			{
				if (key.Equals(variable))
				{
					valid = true;
				}
			}
			if(!valid)
			{
				//No key exist corrosponding to variable.
			}
			return valid;
		}
		/// <summary>
		/// Executes an <see cref="SQLiteCommand"/>.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The number of rows inserted/updated/affected by the command.</returns>
		/// <exception cref="SQLiteException"></exception>
		public int ExecuteCommand(string command)
		{
			if (connection == null)
				throw new SQLiteException("Trying to run SQLite command without an open connection. Use Repository.Open() before running any commands.");
			var cmd = new SQLiteCommand(command, (SQLiteConnection)connection);
			return cmd.ExecuteNonQuery();
		}
		/// <summary>
		/// Executes a queue of commands.
		/// </summary>
		/// <param name="commands">The command to execute.</param>
		/// <returns>The number of rows inserted/updated/affected by the command.</returns>
		/// <exception cref="SQLiteException"></exception>
		public int ExecuteCommmandQueue(Queue<string> commands)
		{
			if (connection == null)
				throw new SQLiteException("Trying to run SQLite command without an open connection. Use Repository.Open() before running any commands.");
			int affections = 0;
			while(commands.Count > 0)
			{
				string command = commands.Dequeue();
				affections += ExecuteCommand(command);
			}
			return affections;
		}
		/// <summary>
		/// Executes an <see cref="SQLiteCommand"/>.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The <see cref="SQLiteDataReader"/> provided by the execution of the command.</returns>
		/// <exception cref="SQLiteException"></exception>
		public SQLiteDataReader ExecuteReader(string command)
		{
			if (connection == null)
				throw new SQLiteException("Trying to run SQLite command without an open connection. Use Repository.Open() before running any commands.");
			var cmd = new SQLiteCommand(command, (SQLiteConnection)connection);
			return cmd.ExecuteReader();
		}
		/// <summary>
		/// Creates a new data table if it does not already exist using the keys provided by the from the <typeparamref name="T"/>.
		/// </summary>
		protected void CreateDataTables()
		{
			string data = "(ID INTEGER PRIMARY KEY," + dataValues.Remove(0, 1);
			ExecuteCommand($"CREATE TABLE IF NOT EXISTS {name} {data}");
		}

		#endregion

		#region Add
		/// <summary>
		/// Adds an <paramref name="element"/> to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="element">The object to be added to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.</param>
		public virtual void Add(T element)
		{
			string obj = BuildElementValues(element);
			Add(obj);
		}
		/// <summary>
		/// Adds an <paramref name="element"/> to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="element">The object to be added, using a string format, to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.</param>
		public virtual void Add(string element)
		{
			if (!element.StartsWith('('))
				element = element.Insert(0, "(");
			if (!element.EndsWith(')'))
				element = element.Insert(element.Length, ")");
			if (Count >= Capacity)
			{
				return;
			}
			int result = ExecuteCommand($"INSERT INTO {name} {dataValues} VALUES {element};");
			if (result > 0)
				AddElement();
		}
		/// <summary>
		/// Adds a range of <paramref name="elements"/> to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="elements"></param>
		public virtual void AddRange(IEnumerable<T> elements)
		{
			foreach (T element in elements)
			{
				Add(element);
			}
		}
		/// <summary>
		/// Adds the multiple of the specified <paramref name="element"/> to the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="element">The object to be added to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.</param>
		/// <param name="count">The amount of objects of the same type to be added to the end of the <see cref="Cosmos.SQLite.Repository{T}"/>.</param>
		public virtual void AddRange(T element, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Add(element);
			}
		}

		#endregion

		#region Update, Replace, Insert
		/// <summary>
		/// Replaces all values of the element at <paramref name="index"/> with the values of the given <paramref name="element"/> in the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element which values will be replaced.</param>
		/// <param name="element">The <see langword="new"/> object that will replace the previous object.</param>
		public virtual void Replace(int index, T element)
		{
			ExecuteCommand($"UPDATE {name} set {dataValues} = {BuildElementValues(element)} WHERE ID = {index + 1}");
		}
		/// <summary>
		/// Updates an elements <paramref name="variable"/> with <paramref name="value"/> at the corrosponding <paramref name="index"/> in the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element which variable will be updated.</param>
		/// <param name="variable">The specified variable name, or identifier if using a <see cref="Cosmos.SQLite.SerialisedValue"/>, to replace (case sensitive).</param>
		/// <param name="value">The <see langword="new"/> value that will replace the old one.</param>
		public virtual void Update(int index, string variable, string value)
		{
			if (!IsValidKey(variable))
			{
				string validKeys = "";
				foreach (string key in dataKeys)
					validKeys += key + " ";
				throw new ArgumentException($"{variable} is not a valid key, identifiers that are allowed consist of: {validKeys}.");
			}
			ExecuteCommand($"UPDATE {name} set {variable} = '{value}' WHERE ID = {index + 1};");
		}
		/// <summary>
		/// Inserts the <paramref name="element"/> into the <see cref="Cosmos.SQLite.Repository{T}"/> at the specified <paramref name="index"/>. All subsequences elements have their index pushed by one.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="element"/> should be inserted.</param>
		/// <param name="element">The object to insert.</param>
		public virtual void Insert(int index, T element)
		{
			if(index < 0 || index >= Count)
			{
				return;
			}
			int toRemove = Count - index;
			var copy = GetRange(index, toRemove);
			RemoveRange(index, toRemove);
			Add(element);
			copy.ToList().ForEach((item) => Add(item));
		}
		#endregion

		#region Remove
		/// <summary>
		/// Returns a generic <see langword="DELETE"/> <see cref="SQLiteCommand"/> as a string, corrosponding to removing an element from the <see cref="Cosmos.SQLite.Repository{T}"/> at the given <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <returns></returns>
		protected string RemoveAtIndexCommand(int index) => $"DELETE from {name} WHERE ID = {index + 1};";
		/// <summary>
		/// Removes the first occurrence of an element that matches the <paramref name="value"/> of the specified <paramref name="variable"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="variable">The specified variable name, or identifier if using a <see cref="Cosmos.SQLite.SerialisedValue"/>, to look for  (case sensitive).</param>
		/// <param name="value">The specified value that will be matched.</param>
		public virtual void Remove(string variable, string value)
		{
			RemoveAt(Get(variable, value).ID);
		}
		/// <summary>
		/// Removes the first occurrence of an element that matches the conditions defined by the specified <see cref="System.Predicate{T}"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="System.Predicate{T}"/> delegate that defines the conditions of the element to remove.</param>
		public virtual void Remove(Predicate<T> match)
		{
			RemoveAt(Get(match).ID);
		}
		/// <summary>
		/// Removes the element at the specified <paramref name="index"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public virtual void RemoveAt(int index)
		{
			int result = ExecuteCommand(RemoveAtIndexCommand(index));
			if (result > 0)
				RemoveElement();
			UpdateElementIndexPositions();
		}
		/// <summary>
		/// Removes all the elements that match the <paramref name="value"/> of the specified <paramref name="variable"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="variable">The specified variable name, or identifier if using a <see cref="Cosmos.SQLite.SerialisedValue"/>, to look for  (case sensitive).</param>
		/// <param name="value">The specified value that will be matched.</param>
		public virtual void RemoveAll(string variable, string value)
		{
			Queue<string> commands = new Queue<string>();
			foreach(var item in GetAll(variable, value))
			{
				commands.Enqueue(RemoveAtIndexCommand(item.ID));
				RemoveElement();
			}
			ExecuteCommmandQueue(commands);
			UpdateElementIndexPositions();
		}
		/// <summary>
		/// Removes all the elements that match the conditions defined by the specified <see cref="System.Predicate{T}"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="System.Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
		public virtual void RemoveAll(Predicate<T> match)
		{
			Queue<string> commands = new Queue<string>();
			foreach(var item in GetAll(match))
			{
				commands.Enqueue(RemoveAtIndexCommand(item.ID));
				RemoveElement();
			}
			ExecuteCommmandQueue(commands);
			UpdateElementIndexPositions();
		}
		/// <summary>
		/// Removes a range of elements from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public virtual void RemoveRange(int index, int count)
		{
			Queue<string> commands = new Queue<string>();
			int itemCount = Count;
			for (int i = index; i < index + count; i++)
			{
				if (i >= itemCount)
					break;
				commands.Enqueue(RemoveAtIndexCommand(i));
			}
			ExecuteCommmandQueue(commands);
			UpdateElementIndexPositions();
		}

		#endregion

		#region Get
		/// <summary>
		/// Returns the element at the specified <paramref name="index"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		public virtual T Get(int index)
		{
			if(index < 0 || index >= Count)
			{
				return default(T);
			}
			var reader = ExecuteReader($"SELECT * from {name} WHERE ID = {index + 1};");
			var result = mapper.MapDataReaderToList(reader);
			return result[0];
		}

		/// <summary>
		/// Searches for an element that matches the <paramref name="value"/> of the specified <paramref name="variable"/>, and returns the first occurrence within the entire <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="variable">The specified variable name, or identifier if using a <see cref="Cosmos.SQLite.SerialisedValue"/>, to look for  (case sensitive).</param>
		/// <param name="value">The specified value that will be matched.</param>
		/// <returns>The first element that matches the conditions defined from the <see cref="Cosmos.SQLite.Repository{T}{T}"/>, it it exists. Otherwise, the default value for type <typeparamref name="T"/>.</returns>
		public virtual T Get(string variable, string value)
		{
			if (!IsValidKey(variable))
				return default(T);
			var result = GetAll(variable, value).ToArray();
			return result.Length > 0 ? result[0] : default(T);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified <see cref="System.Predicate{T}"/>, and returns the first occurrence within the entire <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="System.Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
		/// <returns>The first element that matches the conditions defined by the specified <see cref="System.Predicate{T}"/>, it it exists. Otherwise, the default value for type <typeparamref name="T"/>.</returns>
		public virtual T Get(Predicate<T> match)
		{
			return GetAll().ToList().Find(match);
		}

		/// <summary>
		/// Retrieves all the elements that match the <paramref name="value"/> of the specified <paramref name="variable"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="variable">The specified variable name, or identifier if using a <see cref="Cosmos.SQLite.SerialisedValue"/>, to look for  (case sensitive).</param>
		/// <param name="value">The specified value that will be matched.</param>
		/// <returns>A <see cref="System.Collections.Generic.IEnumerable{T}"/> containing all the elements that match the conditions defined from the <see cref="Cosmos.SQLite.Repository{T}"/>.</returns>
		public virtual IEnumerable<T> GetAll(string variable, string value)
		{
			if(!IsValidKey(variable))
				return Enumerable.Empty<T>();
			var reader = ExecuteReader($"SELECT * from {name} WHERE {variable} = '{value}';");
			var result = mapper.MapDataReaderToList(reader);
			return result;
		}

		/// <summary>
		/// Retrieves all the elements that match the conditions defined by the specified <see cref="System.Predicate{T}"/> from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="match">The <see cref="System.Predicate{T}"/> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>A <see cref="System.Collections.Generic.IEnumerable{T}"/> containing all the elements that match the conditions defined by the specified <see cref="System.Predicate{T}"/>.</returns>
		public virtual IEnumerable<T> GetAll(Predicate<T> match)
		{
			return GetAll().ToList().FindAll(match);
		}

		/// <summary>
		/// Returns all elements from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <returns>A <see cref="System.Collections.Generic.IEnumerable{T}"/> containing all the elements from the <see cref="Cosmos.SQLite.Repository{T}"/>.</returns>
		public virtual IEnumerable<T> GetAll()
		{
			var reader = ExecuteReader($"SELECT * from {name};");
			var result = mapper.MapDataReaderToList(reader);
			return result;
		}

		/// <summary>
		/// Returns a range of elements from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="index">The zero-based index at which the range starts.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <returns></returns>
		public virtual IEnumerable<T> GetRange(int index, int count)
		{
			return GetAll().ToList().FindAll(item => item.ID >= index && item.ID < index + count);
		}

		public virtual bool Exists(Predicate<T> match)
		{
			return GetAll().ToList().Exists(match);
		}

		public virtual int GetIndex(Predicate<T> match)
		{
			return GetAll().ToList().FindIndex(match);
		}

		#endregion

		#region Repository Handles

		/// <summary>
		/// Renames the data table of the <see cref="Cosmos.SQLite.Repository{T}"/> to <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The new name of the <see cref="Cosmos.SQLite.Repository{T}"/> spaces will automatically be replaced by '_' (underscore).</param>
		public void Rename(string name)
		{
			string functionalName = name.Replace(" ", "_");
			ExecuteCommand($"ALTER TABLE {this.name} rename to {functionalName}");
			this.name = functionalName;
		}

		/// <summary>
		/// Must be invoked when adding an element to the data table, otherwise Count of the <see cref="Cosmos.SQLite.Repository{T}"/> can end up out of sync and lead to an <see cref="System.Exception"/>.
		/// </summary>
		protected void AddElement()
		{
			count++;
		}

		/// <summary>
		/// Must be invoked when removing an element from the data table, otherwise Count of the <see cref="Cosmos.SQLite.Repository{T}"/> can end up out of sync and lead to an <see cref="System.Exception"/>.
		/// </summary>
		protected void RemoveElement()
		{
			count--;
		}

		/// <summary>
		/// Copies and clears the entire data table to reorder all elements to their correct index.
		/// </summary>
		protected void UpdateElementIndexPositions()
		{
			var copy = GetAll();
			Clear();
			foreach (var element in copy)
			{
				Add(element);
			}
		}
		/// <summary>
		/// Insures the capacity of the <see cref="Cosmos.SQLite.Repository{T}"/>, will remove excess elements if the capacity limit is set lower than the amount of elements in the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		protected void InsureCapacity()
		{
			if(Count > Capacity)
			{
				RemoveRange(Capacity, Count - Capacity);
			}
		}

		/// <summary>
		/// To read or write from or to the data table <see cref="Cosmos.SQLite.Repository{T}{T}.Open"/> must be invoked before any actions can be performed.
		/// </summary>
		public void Open()
		{
			if (connection == null)
			{
				connection = provider.CreateConnection();
				connection.Open();
			}
			else
			{
				//Debug.Log("Repository.Open() should not be invoked while a connection is already established.", LogOption.Warning)
				return;
			}
			CreateDataTables();
		}

		/// <summary>
		/// Closes any data stream and connection to the <see cref="Cosmos.SQLite.Repository{T}"/>, stopping all further read or write actions to the data table.
		/// </summary>
		public void Close()
		{
			if (connection != null)
			{
				connection.Close();
				connection = null;
			}
			else
			{
				//Debug.Log("Repository.Close() cannot be invoked unless a connection is already established.", LogOption.Warning)
				return;
			}
		}

		/// <summary>
		/// Removes all elements from the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		public void Clear()
		{
			ExecuteCommand($"DELETE from {name}");
			count = 0;
		}

		/// <summary>
		/// Deletes and disposes of the data table and <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		public void Drop()
		{
			Dispose();
			ExecuteCommand($"DROP table {name}");
		}
		#endregion

		#region Export / Import
		/// <summary>
		/// Exports the data table of the <see cref="Cosmos.SQLite.Repository{T}"/> to the given <paramref name="path"/>, will be exported as a .csv file with the name of the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="path">The directory location of where to export the file, if it's empty the base directory for the application will be used.</param>
		public void Export(string path = "")
		{
			if (string.IsNullOrEmpty(path))
			{
				path = BaseDirectory;
			}
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			if (!path.EndsWith("/") || !path.EndsWith("\\"))
				path += "/";

			var elements = GetAll();
			string filename = $"{Name}.csv";
			StreamWriter writer = new StreamWriter($"{path}{filename}");
			StringBuilder sb = new StringBuilder();
			foreach (var element in elements)
			{
				sb.Clear();
				bool initalValue = true;
				string item = BuildElementValues(element);
				item = item.Remove(item.Length - 1, 1).Remove(0, 1);
				string[] csv = item.Split(',');
				foreach(string s in csv)
				{
					if(initalValue)
						initalValue = false;
					else
						sb.Append(",");
					sb.Append(s.Replace("'", "").Trim());
				}
				writer.WriteLine(sb);
			}
			writer.Close();
		}
		/// <summary>
		/// Imports data from a file from given path <paramref name="path"/> (filename included) and adds elements to the <see cref="Cosmos.SQLite.Repository{T}"/>, must be a .csv file to be supported by the importer.
		/// </summary>
		/// <param name="path">The directory location and filename to import.</param>
		/// <exception cref="FileFormatException"></exception>
		public void Import(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			if (connection == null)
				Open();

			string[] split = path.Split('/');
			string[] fileExtension = split[split.Length - 1].Split('.');
			if (fileExtension[fileExtension.Length - 1] != "csv")
			{
				throw new FormatException($"Unsupported file extension ({fileExtension[fileExtension.Length - 1]}) - only csv files are supported for import.");
			}

			StreamReader reader = new StreamReader(path);
			string? line;
			StringBuilder sb = new StringBuilder();
			while((line = reader.ReadLine()) != null)
			{
				sb.Clear();
				bool initalValue = true;
				string[] csv = line.Split(",");
				foreach(string s in csv)
				{
					if (initalValue)
						initalValue = false;
					else
						sb.Append(",");

					if (!s.StartsWith("'"))
						sb.Append("'");
					sb.Append(s);
					if (!line.EndsWith("'"))
						sb.Append("'");
				}
				line = sb.ToString();
				if (!line.StartsWith('('))
					line = line.Insert(0, "(");
				if (!line.EndsWith(')'))
					line = line.Insert(line.Length, ")");
				Add(line);
			}
			reader.Close();
		}

		#endregion

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				// TODO: dispose managed state (managed objects).
				Clear();
			}
			Close();

			// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
			// TODO: set large fields to null.

			disposed = true;
		}
	}
}