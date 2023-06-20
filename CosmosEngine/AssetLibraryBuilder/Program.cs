// See https://aka.ms/new-console-template for more information
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");

string s1 = AppDomain.CurrentDomain.BaseDirectory;
string s2 = Assembly.GetExecutingAssembly().Location;
string s3 = System.IO.Directory.GetCurrentDirectory();
var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

Console.WriteLine($"AppDomain.CurrentDomain.BaseDirectory: {s1}");
Console.WriteLine($"Assembly.GetExecutingAssembly().Location: {s2}");
Console.WriteLine($"System.IO.Directory.GetCurrentDirectory(): {s3}");
Console.WriteLine($"DirectoryInfo(Directory.GetCurrentDirectory()): {directory.Parent.FullName}");