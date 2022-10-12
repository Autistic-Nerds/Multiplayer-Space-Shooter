using System;

namespace CosmosEngine
{
	/// <summary>
	/// The RequireComponent attribute automatically adds required components as dependencies.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class RequireComponent : Attribute
	{
		private Type[] requiredComponents;
		public Type[] RequiredComponents => requiredComponents;

		/// <summary>
		/// When a <see cref="CosmosEngine.Component"/> which uses <see cref="CosmosEngine.RequireComponent"/> to a <see cref="CosmosEngine.GameObject"/>, the required component is automatically added to the GameObject. RequireComponent checks for missing dependencies when using <see cref="CosmosEngine.GameObject.AddComponent{T}"/> and <see cref="CoreModule.Object.Destroy(CoreModule.Object)"/> on a component. If an attached components is a dependency that component can't be destroyed.
		/// </summary>
		/// <param name="requiredComponents"></param>
		public RequireComponent(params Type[] requiredComponents)
		{
			this.requiredComponents = requiredComponents;
		}
	}
}