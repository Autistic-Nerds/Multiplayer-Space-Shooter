using CosmosEngine;
using System;
using System.Reflection;

namespace Cosmos.SQLite
{
	/// <summary>
	/// Marks a field as serialisable by the <see cref="Cosmos.SQLite.Mapper{T}"/> or <see cref="Cosmos.SQLite.Repository{T}"/>, by using the <see cref="Cosmos.SQLite.SerialisedValue"/> will automatically store the field as a value when using it in a <see cref="Cosmos.SQLite.Repository{T}"/>, it's possible to store values that are not marked as <see cref="Cosmos.SQLite.SerialisedValue"/> by creating a custom <see cref="Cosmos.SQLite.Repository{T}"/>. It's possible to set a unique identifier for the field, if none is provided the field's name will be used as the identifier. Any variable that is not a primitive must use a custom converter inherting from <see cref="System.ComponentModel.TypeConverter"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SerialisedValue : Attribute
	{
		private static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		private string identifier = string.Empty;
		public string Identifier => identifier;
		public SerialisedValue() { }
		/// <summary>
		/// Spaces will automatically be replaced by '_' (underscore).
		/// </summary>
		/// <param name="identifier"></param>
		public SerialisedValue(string identifier)
		{
			this.identifier = identifier.Replace(" ", "_");
		}
		/// <summary>
		/// Returns true if the <paramref name="field"/> is serialisable by the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object containing the field.</typeparam>
		/// <param name="field">The field to check.</param>
		public static bool IsSerialisableField<T>(FieldInfo field) => IsSerialisableField<T>(field, out _);
		/// <summary>
		/// Returns true if the <paramref name="field"/> is serialisable by the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the object containing the field.</typeparam>
		/// <param name="field">The field to check.</param>
		/// <param name="serialisedValue">The <see cref="Cosmos.SQLite.SerialisedValue"/> associated with the field. Can be <see langword="null"/> if the field does not implement the <see cref="Cosmos.SQLite.SerialisedValue"/> attribute. Field may still be serialisable if the object owning the field implements the <see cref="Cosmos.SQLite.SerialisedMember"/> attribute.</param>
		public static bool IsSerialisableField<T>(FieldInfo field, out SerialisedValue? serialisedValue) => IsSerialisableField(field, typeof(T), out serialisedValue);
		/// <summary>
		/// Returns true if the <paramref name="field"/> is serialisable by the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="field">The field to check.</param>
		/// <param name="type">The type of the object containing the field.</param>
		public static bool IsSerialisableField(FieldInfo field, Type type) => IsSerialisableField(field, type, out _);
		/// <summary>
		/// Returns true if the <paramref name="field"/> is serialisable by the <see cref="Cosmos.SQLite.Repository{T}"/>.
		/// </summary>
		/// <param name="field">The field to check.</param>
		/// <param name="type">The type of the object containing the field.</param>
		/// <param name="serialisedValue">The <see cref="Cosmos.SQLite.SerialisedValue"/> associated with the field. Can be <see langword="null"/> if the field does not implement the <see cref="Cosmos.SQLite.SerialisedValue"/> attribute. Field may still be serialisable if the object owning the field implements the <see cref="Cosmos.SQLite.SerialisedMember"/> attribute.</param>
		public static bool IsSerialisableField(FieldInfo field, Type type, out SerialisedValue? serialisedValue)
		{
			serialisedValue = field.GetCustomAttribute<SerialisedValue>(true);
			if (field.Name.Contains("k__backingfield", StringComparison.OrdinalIgnoreCase))
				return false;
			if (serialisedValue != null || SerialisedMember.IsDefined(type))
			{
				return true;
			}
			return false;
		}

		public static FieldInfo GetSerialisableField<T>(string identifier) => GetSerialisableField(identifier, typeof(T));

		public static FieldInfo GetSerialisableField(string identifier, Type type)
		{
			if (type == null)
				return null;

			foreach(FieldInfo field in type.GetFields(bindingFlags))
			{
				if (IsSerialisableField(field, type, out SerialisedValue serialisedValue))
				{
					if ((serialisedValue != null && identifier.Equals(serialisedValue.identifier)) || identifier.Equals(field.Name))
							return field;
				}
			}
			return default;
		}

		public static FieldInfo[] GetSerialisableFields<T>() => GetSerialisableFields(typeof(T));

		public static FieldInfo[] GetSerialisableFields(Type type)
		{
			if (type == null)
				return null;
			return null;
		}
	}
}