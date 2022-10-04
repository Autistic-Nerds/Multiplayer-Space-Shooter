using System;

namespace CosmosEngine
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class KeepReference : Attribute
	{

	}
}