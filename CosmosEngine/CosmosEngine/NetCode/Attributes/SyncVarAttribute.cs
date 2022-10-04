using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class SyncVarAttribute : Attribute
{
	private bool forceSync;
	public string hook;

	public bool ForceSync => forceSync;
	public SyncVarAttribute()
	{

	}

	public SyncVarAttribute(bool forceSync)
	{
		this.forceSync = forceSync;
	}
}
