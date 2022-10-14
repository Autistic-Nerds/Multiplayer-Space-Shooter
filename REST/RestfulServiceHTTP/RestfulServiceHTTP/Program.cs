using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Json;
using System.Text;

Console.WriteLine($"write the port you wish to post/listen to.");
short port;
while(true)
{
	string? portChoice = Console.ReadLine();
	if(short.TryParse(portChoice, out port))
	{
		break;
	}
	else
	{
		Console.WriteLine($"Invalid port, please try again");
	}
}

HttpClient client = new HttpClient();
string url = $"https://localhost:{port}/api/score";

HelpCenter();

while (true)
{
	string? readline = Console.ReadLine();
	if (readline == null)
		break;
	if (readline.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
		break;
	if(readline.Equals("help", StringComparison.CurrentCultureIgnoreCase))
	{
		HelpCenter();
		break;
	}

	string path = url;
	if (readline.Contains("get", StringComparison.CurrentCultureIgnoreCase))
	{
		string[] split = readline.Split(' ');
		if(split.Length > 1)
		{
			path += $"/{split[1]}";
		}
		HttpResponseMessage respone = await client.GetAsync(path);
		Console.WriteLine(respone);
		string responeBody = await respone.Content.ReadAsStringAsync();
		Console.WriteLine(responeBody);

		//var responesJson = await respone.Content.ReadFromJsonAsync<Score[]>();
		//if (responesJson != null)
		//{
		//	foreach (Score s in responesJson)
		//	{
		//		Console.WriteLine($"{s.Name}: {s.ScoreNumber}");
		//	}
		//}
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
	else
	{
		Console.WriteLine($"Invalid input, please try again.");
	}
}

void HelpCenter()
{
	Console.WriteLine($"To use this application, type your desired input.");
	Console.WriteLine($"get - show a collection of all the values.");
	Console.WriteLine($"get [name] - show the [score] with the given [name].");
	Console.WriteLine($"post [name] [score] - post a [score] with the given [name].");
	Console.WriteLine($"put [name] [score] - modifies the [score] of the given [name].");
	Console.WriteLine($"delete [name] - deletes the [score] at [name].");
	Console.WriteLine($"help - show these helpful tooltips.");
	Console.WriteLine($"exit - closes the application.");
	Console.WriteLine();
}