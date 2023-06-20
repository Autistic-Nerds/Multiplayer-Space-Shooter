using System;

public abstract class SingleInstance<T> where T : SingleInstance<T>
{
	private T? instance;
	public T? Instance => instance ??= Activator.CreateInstance<T>();

}

public class Map : SingleInstance<Map>
{
	private Map() { }
}

public class World
{
	private void Start()
	{

	}
}