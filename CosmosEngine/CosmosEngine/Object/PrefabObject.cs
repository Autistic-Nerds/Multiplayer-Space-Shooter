
using System.Collections.Generic;

namespace CosmosEngine
{
	[System.Obsolete("Not functional", false)]
	public class PrefabObject
	{
		private string name;
		private List<Component> components;

		public string Name { get => name; set => name = value; }

		public PrefabObject(string name)
		{
			this.name = name;
		}

		public T AddComponent<T>() where T : Component, new()
		{
			T component = new T();
			components.Add(component);
			return component as T;
		}

		public T GetComponent<T>() where T : Component
		{
			return components.Find(item => (item.GetType() == typeof(T) || item.GetType().IsSubclassOf(typeof(T))) && !item.Expired) as T;
		}
	}
}