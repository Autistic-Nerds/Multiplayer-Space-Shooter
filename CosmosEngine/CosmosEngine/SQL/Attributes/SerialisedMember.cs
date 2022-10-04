using System;
using System.Reflection;

namespace Cosmos.SQLite
{
	/// <summary>
	/// The <see cref="Cosmos.SQLite.SerialisedMember"/> attribute marks all the object's fields as <see cref="Cosmos.SQLite.SerialisedValue"/>. This will automatically generate keys for all the object's fields when it's used in a <see cref="Cosmos.SQLite.Repository{T}"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class SerialisedMember : Attribute
	{
		public static bool IsDefined(MemberInfo element) => IsDefined(element, typeof(SerialisedMember));
	}
}