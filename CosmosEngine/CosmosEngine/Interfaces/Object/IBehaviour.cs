
using System;

namespace CosmosEngine.CoreModule
{
	public interface IBehaviour : IObject
	{
		Transform Transform { get; }
#nullable enable
		T? AddComponent<T>() where T : Component;
		Component? AddComponent(Type componentType);
		T? GetComponent<T>() where T : class;
		Component? GetComponent(Type componentType);
		T[] GetComponents<T>() where T : class;
		Component[] GetComponents(Type componentType);
#nullable disable
	}
}