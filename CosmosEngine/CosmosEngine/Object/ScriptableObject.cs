
using CosmosEngine.CoreModule;
using System.Collections.Generic;

namespace CosmosEngine
{
	public abstract class ScriptableObject : Object
	{
		private static List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();

		~ScriptableObject()
		{
			ClearInstance(GetType());
		}

		public static T Instance<T>() where T : ScriptableObject, new() => Instance(typeof(T)) as T;

		public static ScriptableObject Instance(System.Type type)
		{
			ScriptableObject instance;
			int index = FindIndex(type);
			if (index == -1)
			{
				//instance does not exist and must be created.
				instance = (ScriptableObject)System.Activator.CreateInstance(type);
				scriptableObjects.Add(instance);
			}
			else
				instance = scriptableObjects[index];
			return instance;
		}

		public static void ClearInstance<T>() => ClearInstance(typeof(T));
		public static void ClearInstance(System.Type type)
		{
			int index = FindIndex(type);
			if (index != -1)
				scriptableObjects.RemoveAt(index);
		}

		private static int FindIndex(System.Type type) => scriptableObjects.FindIndex(item => item.GetType() == type);
	}
}