
using CosmosEngine.CoreModule;
using System;
using System.Reflection;

namespace CosmosEngine
{
	public static class TypeExtension
	{
		public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		/// <summary>
		/// Returns <see langword="true"/> if the <paramref name="methodName"/> exists on the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static bool MethodExistsOnObject(this Type type, string methodName)
		{
			bool methodExists = false;
			Type t = type;
			MethodInfo method = type.GetMethod(methodName, DefaultFlags);
			do
			{
				if (method != null && method.DeclaringType == t)
				{
					methodExists = true;
					break;
				}
				t = t.BaseType;
			} while (t != typeof(Behaviour));
			return methodExists;
		}
	}
}