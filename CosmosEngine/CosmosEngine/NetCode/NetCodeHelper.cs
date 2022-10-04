using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class NetCodeHelper
{
	public static bool IsSyncVar(this FieldInfo field)
	{
		object[] fieldMarkers = field.GetCustomAttributes(typeof(SyncVarAttribute), true);
		return fieldMarkers.Length > 0;
	}
}
