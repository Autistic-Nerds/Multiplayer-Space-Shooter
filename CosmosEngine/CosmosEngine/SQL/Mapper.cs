using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Cosmos.SQLite
{
	public class Mapper<T> : IMapper<T> where T : IRepositoryElement, new() 
	{
		public virtual List<T> MapDataReaderToList(IDataReader reader)
		{
			List<T> result = new List<T>();
			while (reader.Read())
			{
				T element = new T();
				element.ID = reader.GetInt32(0) - 1;
				FieldInfo[] fieldInfos = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				for(int i = 0; i < fieldInfos.Length; i++)
				{
					FieldInfo field = fieldInfos[i];
					if (SerialisedValue.IsSerialisableField<T>(field, out SerialisedValue serialisedValue))
					{
						string value = reader.GetString(i + 1);
						if (field.FieldType == typeof(string))
						{
							field.SetValue(element, value);
						}
						else
						{
							var converter = TypeDescriptor.GetConverter(field.FieldType);
							if (converter == null)
								throw new NullReferenceException($"No converter for the type {field.FieldType} exist - this value type cannot be used for the repository");
							field.SetValue(element, converter.ConvertFromString(value));
						}
					}
				}
				result.Add(element);
			}
			return result;
		}
	}
}