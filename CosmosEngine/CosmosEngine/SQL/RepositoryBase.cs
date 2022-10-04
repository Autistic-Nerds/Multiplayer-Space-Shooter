using System.IO;

namespace Cosmos.SQLite
{
	public abstract class Repository
	{
		public static string BaseDirectory => $"{System.AppDomain.CurrentDomain.BaseDirectory}";
		public static Repository<T> Create<T>(string name) where T : IRepositoryElement, new() => Create<T>(name, int.MaxValue);
		public static Repository<T> Create<T>(string name, int capacity) where T : IRepositoryElement, new() => Create(name, capacity, System.Array.Empty<T>());
		public static Repository<T> Create<T>(string name, int capacity, params T[] elements) where T : IRepositoryElement, new()
		{
			IDatabaseProvider provider = DatabaseProvider.Create();
			IMapper<T> mapper = new Mapper<T>();
			Repository<T> repository = new Repository<T>(name, provider, mapper, capacity);
			repository.Open();
			foreach (T e in elements)
				repository.Add(e);
			return repository;
		}

		public static Repository<T> CreateFromFile<T>(string path) where T : IRepositoryElement, new() => CreateFromFile(path, System.Array.Empty<T>());
		public static Repository<T> CreateFromFile<T>(string path, params T[] elements) where T : IRepositoryElement, new()
		{
			string[] split = path.Split('/');
			string[] file = split[split.Length - 1].Split('.');
			string name = file[0];
			Repository<T> repository = Create<T>(name);
			if (File.Exists(path))
			{
				repository.Import(path);
			}
			foreach (T e in elements)
				repository.Add(e);
			return repository;
		}

	}
}