using System;
using System.Reflection;

namespace CosmosEngine.Netcode
{
	/// <summary>
	/// Mark a field as a <see cref="CosmosEngine.Netcode.SyncVarAttribute"/> to automatically update and synchronize to and from the server, whenever a change happens. <see cref="CosmosEngine.Netcode.SyncVarAttribute"/> will always be synchronized by the authority of the <see cref="CosmosEngine.Netcode.NetcodeIdentity"/> to the server and from the server to the rest of the clients.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SyncVarAttribute : Attribute
	{
		private bool forceSync;
		/// <summary>
		/// Represents a method to be invoked whenever the variable have changed. The method may be represented in three different forms. If multiple overloads exist of the method the one with no or fewest parameters will always be chosen.
		/// <list type="bullet">
		/// <item>With no parameters. ExampleMethod();</item>
		/// <item>With one parameter corrosponding to the variable type. ExampleMethod(T var);</item>
		/// <item>With two parameter corrosponding to the variable type. ExampleMEthod(T old, T new);</item>
		/// </list>
		/// Could be used if a change between the two variables are important to notice.
		/// </summary>
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
}