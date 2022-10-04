
using CosmosEngine.CoreModule;
using CosmosEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CosmosEngine
{
	public static class Prefab
	{
		/// <summary>
		/// <inheritdoc cref="CosmosEngine.Prefab.Create{T}(GameObject)"/>
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static GameObject Create(GameObject original)
		{
			return Create<GameObject>(original);
		}

		/// <summary>
		/// Creates a new <see cref="CosmosEngine.GameObject"/> from an already existing <see cref="CosmosEngine.GameObject"/>, this will copy all components, with the same values and data, from the original object to a new object. <typeparamref name="T"/> is the an object that is attached to the <see cref="CosmosEngine.GameObject"/>, if it's a <see cref="CosmosEngine.Component"/>, then <typeparamref name="T"/> will be added to the <see cref="CosmosEngine.GameObject"/>. It's possible to create unique rules when making a prefab of a class, inherit from the <see cref="CosmosEngine.IReplicatable{T}"/>.
		/// </summary>
		/// <param name="original">The original <see cref="GameObject"/> that is being cloned.</param>
		/// <returns>The new instantiated <see cref="GameObject"/></returns>
		public static T Create<T>(GameObject original) where T : CoreModule.Object
		{
			if (original == null)
			{
				Debug.Log($"Trying to a copy of an object when the original object is null.", LogFormat.Error);
				return default(T);
			}
			//We create a new GameObject with the same name as the original, but adding (Clone).
			GameObject newObject = new GameObject(original.Name + " (Clone)");
			//We copy the original's GameObject Enabled state.
			newObject.Enabled = original.Enabled;

			//We attach a Transform to the new Game Object and clone the values from the original object's Transform.
			if (original.Transform is RectTransform)
				newObject.ReplaceTransform<RectTransform>(true);
			else
				newObject.ReplaceTransform<Transform>(true);
			//We copy the original's GameObject Transform.
			newObject.Transform.Copy(original.Transform);

			//We want private and public fields, only from an instantiated member.
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

			//We get an array of all the attached components of the original GameObject.
			original.Finalise();
			Component[] attachedComponents = original.GetComponentsAll();

			//We will loop over all the attached Component and instantiate them as new and copy values from the origianl ones.
			foreach (Component c in attachedComponents)
			{
				if (c == null || c.Expired)
					continue;
				if (c is Transform)
					continue;
				//This is just a fail safe against possibly unsafe while loops.
				int loopFailSafe = 0;

				//We get the type of the attached component.
				Type type = c.GetType();
				//We create a new instance of the component type.
				Component newComponent = (Component)Activator.CreateInstance(type);
				do
				{
					//If we hit CoreModule level we want to break out of the loop, since everything from there holds values we don't wish to copy.
					if (type == typeof(Component) || type == typeof(UIComponent))
						break;
					if (loopFailSafe > 10000)
						break;

					//We get all fieldinfo from the current type we're looking on.
					FieldInfo[] fieldInfo = type.GetFields(flags);
					//Foreach field we find, we copy the value from the original component to the new instance.
					foreach (FieldInfo field in fieldInfo)
					{
						if (field.GetValue(c) == null)
							continue;

						//If the copied field is a class we want to check for the interface IReplicable<T>.
						//If a class contains this interface, it means that the class wish to be replicated a new way, we invoke the method Clone(T).
						//The returned value should be a new object of that class, which means we have a new reference.
						//If IReplicable is not implemented the reference of a class will remain the same.
						if (field.FieldType.IsClass && 
							!Attribute.IsDefined(field, typeof(KeepReference)))
						{
							if(field.FieldType.GetInterfaces().Any(
								item => item.IsGenericType && 
								item.GetGenericTypeDefinition() == typeof(IReplicatable<>)))
							{
								MethodInfo m = field.FieldType.GetMethod("Clone", BindingFlags.Instance | BindingFlags.Public);
								if (m != null)
								{
									object copy = m.Invoke(field.GetValue(c), new object[] { field.GetValue(c) });
									type.GetField(field.Name, flags).SetValue(newComponent, copy);
									continue;
								}
							}
							else if(field.FieldType.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
							{
								//Debug.Log($"GetInterfaces() == IEnumerable --- {field.Name} on {type.Name}");
							}
							continue;
						}

						type.GetField(field.Name, flags).SetValue(newComponent, field.GetValue(c));
					}
					//We next iterate over the next BaseType (type the current type is inheriting from) to not only copy direct values, but copy values from inherited fields.
					type = type.BaseType;
					loopFailSafe++;

				} while (type != null);
				//We copy the original component's Active state.
				newComponent.Enabled = c.ActiveSelf;
				//At the end we add the new component to the new GameObject.
				newObject.AddComponent(newComponent);
			}
			newObject.Transform.TransformUpdateEvent.Invoke();
			if(typeof(T) == typeof(GameObject))
			{
				return newObject as T;
			}
			return (CoreModule.Object)newObject.GetOrAddComponent(typeof(T)) as T;
		}

		/// <summary>
		/// Copies the values from one <see cref="CosmosEngine.Component"/> to another <see cref="CosmosEngine.Component"/> and returns the new component.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="component"></param>
		/// <returns></returns>
		public static T CopyComponent<T>(Component component) where T: Component, new()
		{
			if (component == null || component.Expired)
				return default(T);

			//We want private and public fields, only from an instantiated member.
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

			//This is just a fail safe. (Don't like using while loops, since they can be unsafe)
			int loopFailSafe = 0;
			//We get the type of the attached component.
			Type type = component.GetType();
			//We create a new instance of the component type.
			Component newComponent = (Component)Activator.CreateInstance(type);
			do
			{
				//If we hit CoreModule level we want to break out of the loop, since everything from there holds values we don't wish to copy.
				if (type == typeof(Component))
					break;
				if (loopFailSafe > 1000)
					break;

				//We get all fieldinfo from the current type we're looking on.
				FieldInfo[] fieldInfo = type.GetFields(flags);
				//Foreach field we find, we copy the value from the original component to the new instance.
				foreach (FieldInfo field in fieldInfo)
				{
					if (field.GetValue(component) == null)
						continue;

					//If the copied field is a class we want to check for the interface IReplicable<T>.
					//If a class contains this interface, it means that the class wish to be replicated a new way, we invoke the method Clone(T).
					//The returned value should be a new object of that class, which means we have a new reference.
					//If IReplicable is not implemented the reference of a class will remain the same.
					if (field.FieldType.IsClass)
					{
						if (field.FieldType.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IReplicatable<>)))
						{
							MethodInfo m = field.FieldType.GetMethod("Clone", BindingFlags.Instance | BindingFlags.Public);
							if (m != null)
							{
								object copy = m.Invoke(field.GetValue(component), new object[] { field.GetValue(component) });
								type.GetField(field.Name, flags).SetValue(newComponent, copy);
								continue;
							}
						}
					}
					type.GetField(field.Name, flags).SetValue(newComponent, field.GetValue(component));
				}
				//We next iterate over the next BaseType (type the current type is inheriting from) to not only copy direct values, but copy values from inherited fields.
				type = type.BaseType;
				loopFailSafe++;

			} while (type != null);
			return newComponent as T;
		}
	}
}