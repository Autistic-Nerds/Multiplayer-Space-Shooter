using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CosmosEngine.Netcode
{
	public static class NetcodeHelper
	{
		public static bool IsSyncVar(this FieldInfo field)
		{
			object[] fieldMarkers = field.GetCustomAttributes(typeof(SyncVarAttribute), true);
			return fieldMarkers.Length > 0;
		}
	}
}