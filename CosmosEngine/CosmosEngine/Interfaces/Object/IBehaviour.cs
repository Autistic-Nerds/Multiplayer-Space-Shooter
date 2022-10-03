//Written by Philip Wittusen
using System;

namespace CosmosEngine.CoreModule
{
	public interface IBehaviour : IObject
	{
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