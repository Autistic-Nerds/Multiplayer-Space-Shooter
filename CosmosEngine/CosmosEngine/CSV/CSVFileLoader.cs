
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cosmos
{
	public static class CSVFileLoader
	{
		public static string BaseDirectory => $"{System.AppDomain.CurrentDomain.BaseDirectory}";

		public static string[] ReadFile(string path) => ReadFile(path, ReadFileOption.None);

		public static string[] ReadFile(string path, ReadFileOption option)
		{
			List<string> list;
			ReadFile(path, option, out list);
			return list.ToArray();
		}

		public static void ReadFile(string path, ReadFileOption option, out List<string> output)
		{
			output = new List<string>();
			List<string> source = new List<string>();
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"Could not find specific file at {path}");
			}
			StreamReader reader = new StreamReader(path);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				source.Add(line);
			}

			if (!option.HasFlag(ReadFileOption.KeepEmpty))
			{
				source.RemoveAll(item => item == null);
			}
			if (option.HasFlag(ReadFileOption.Distinct))
			{
				source = source.Distinct().ToList();
			}
			output.AddRange(source);
		}
	}
}