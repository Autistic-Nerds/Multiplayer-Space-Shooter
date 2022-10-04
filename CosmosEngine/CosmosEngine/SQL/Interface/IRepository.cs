using System;
using System.Collections.Generic;

namespace Cosmos.SQLite
{
	public interface IRepository<T> where T : IRepositoryElement
	{
		string Name { get; set; }
		int Count { get; }
		int Capacity { get; set; }
		string[] Keys { get; }
		T this [int index] { get; set; }

		/// <summary>
		/// Adds <paramref name="element"/> at the end of the repository.
		/// </summary>
		/// <param name="element"></param>
		void Add(T element);
		/// <summary>
		/// Adds a new elements at the end of the repository in a string format.
		/// </summary>
		/// <param name="element"></param>
		void Add(string element);
		/// <summary>
		/// Adds a range of elements to the end of the repository.
		/// </summary>
		/// <param name="elements"></param>
		void AddRange(IEnumerable<T> elements);
		/// <summary>
		/// Adds <paramref name="element"/> to the end of the repository equal to <paramref name="count"/> times (each will be assigned a unique ID).
		/// </summary>
		/// <param name="element"></param>
		/// <param name="count"></param>
		void AddRange(T element, int count);


		/// <summary>
		/// Replaces the element at <paramref name="id"/> with <paramref name="element"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="element"></param>
		void Replace(int id, T element);
		/// <summary>
		/// Overwrite element at <paramref name="id"/>  <paramref name="variable"/> with <paramref name="value"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		void Update(int id, string variable, string value);
		/// <summary>
		/// Inserts a new <paramref name="element"/> at index <paramref name="id"/>, pushing the elements after down by one space. 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="element"></param>
		void Insert(int id, T element);


		/// <summary>
		/// Remove the first element from the repository with matching <paramref name="value"/> of <paramref name="variable"/>.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		void Remove(string variable, string value);
		/// <summary>
		/// Removes the first element matching the predicate from the repository.
		/// </summary>
		/// <param name="element"></param>
		void Remove(Predicate<T> match);
		/// <summary>
		/// Remove an element from the repository at position <paramref name="id"/>.
		/// </summary>
		/// <param name="id"></param>
		void RemoveAt(int id);
		/// <summary>
		/// Removes all elements from the repository with matching <paramref name="value"/> of <paramref name="variable"/>.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		void RemoveAll(string variable, string value);
		/// <summary>
		/// Removes all elements from the repository with matching the given <see cref="System.Predicate{T}"/>.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		void RemoveAll(Predicate<T> match);
		/// <summary>
		/// Removes a range of elements from the repository.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		void RemoveRange(int index, int count);


		/// <summary>
		/// Returns element <typeparamref name="T"/> at position <paramref name="id"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T Get(int id);
		/// <summary>
		/// Returns the first element with matching <paramref name="value"/> of <paramref name="variable"/> from the repository.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		T Get(string variable, string value);
		/// <summary>
		/// Returns the first element matching the predicate from the repository.
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		T Get(Predicate<T> match);
		/// <summary>
		/// Returns all elements with matching <paramref name="value"/> of <paramref name="variable"/> from the repository.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IEnumerable<T> GetAll(string variable, string value);
		/// <summary>
		/// Returns all elements with matching the given <see cref="System.Predicate{T}"/> from the repository.
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		IEnumerable<T> GetAll(Predicate<T> match);
		/// <summary>
		/// Returns all elements from the repository.
		/// </summary>
		/// <returns></returns>
		IEnumerable<T> GetAll();
		/// <summary>
		/// Returns a range of elements from the <paramref name="index"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		IEnumerable<T> GetRange(int index, int count);

		/// <summary>
		/// Renames the repository.
		/// </summary>
		/// <param name="name"></param>
		void Rename(string name);
		/// <summary>
		/// Opens the repository.
		/// </summary>
		void Open();
		/// <summary>
		/// Closes the repository.
		/// </summary>
		void Close();
		/// <summary>
		/// Clears all elements from the repository.
		/// </summary>
		void Clear();
		/// <summary>
		/// Deletes the entire repository.
		/// </summary>
		void Drop();
		/// <summary>
		/// Exports the repository as a CSV file to the given path. If no path is provided it will be placed in the application base directory, will overwrite any file with the same name at the path.
		/// </summary>
		/// <param name="path"></param>
		void Export(string path = "");
		/// <summary>
		/// Imports a file from the given path, and adds all elements to the repository. Only CSV file extension is supported. 
		/// </summary>
		/// <param name="path"></param>
		void Import(string path);
	}
}