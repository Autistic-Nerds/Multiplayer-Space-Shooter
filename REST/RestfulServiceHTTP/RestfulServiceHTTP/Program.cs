using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Json;
using System.Text;

HttpClient client = new HttpClient();
string url = "https://localhost:5001/api/score";

while (true)
{
	string? readline = Console.ReadLine();
	if (readline == null)
		break;
	if (readline.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
		break;

	string path = url;
	if (readline.Contains("get", StringComparison.CurrentCultureIgnoreCase))
	{
		string[] split = readline.Split(' ');
		if(split.Length > 1)
		{
			path += $"/{split[1]}";
		}
		HttpResponseMessage respone = await client.GetAsync(path);
		//Console.WriteLine(respone);
		string responeBody = await respone.Content.ReadAsStringAsync();
		Console.WriteLine(responeBody);

		var responesJson = await respone.Content.ReadFromJsonAsync<Score[]>();
		if (responesJson != null)
		{
			foreach (Score s in responesJson)
			{
				Console.WriteLine($"{s.Name}: {s.ScoreNumber}");
			}
		}
	}
	else if(readline.Contains("post", StringComparison.CurrentCultureIgnoreCase))
	{
		string[] split = readline.Split(' ');
		try
		{
			var score = new Score()
			{
				Name = split[1],
				ScoreNumber = int.Parse(split[2]),
			};
			var data = new StringContent(JsonConvert.SerializeObject(score), Encoding.UTF8, "application/json");
			var respone = await client.PostAsync(url, data);
			Console.WriteLine(respone);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
	else if(readline.Contains("delete", StringComparison.CurrentCultureIgnoreCase))
	{
		string[] split = readline.Split(' ');
		if (split.Length > 1)
		{
			path += $"/{split[1]}";
		}
		var respone = await client.DeleteAsync(path);
		Console.WriteLine(respone);
	}
	else if(readline.Contains($"put"))
	{
		string[] split = readline.Split(' ');
		try
		{
			var score = new Score()
			{
				Name = split[1],
				ScoreNumber = int.Parse(split[2]),
			};
			path += $"/{score.Name}";

			var data = new StringContent(JsonConvert.SerializeObject(score), Encoding.UTF8, "application/json");
			var respone = await client.PutAsync(path, data);
			Console.WriteLine(respone);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
}