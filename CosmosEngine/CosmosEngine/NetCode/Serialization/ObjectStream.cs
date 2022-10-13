using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CosmosEngine.Netcode.Serialization
{
	[System.Obsolete("Have been replaced by NetcodeWriter and NetcodeReader",false)]
	internal class ObjectStream
	{
		private StringBuilder stringBuilder;
		private string objectStream;
		private bool open;

		public bool Empty => string.IsNullOrWhiteSpace(stringBuilder.ToString());
		public string Stream { get => objectStream; internal set => objectStream = value; }

		public void Write(FieldInfo field, object obj) => Write(field.Name, field.GetValue(obj));

		public void Write(string name, object value)
		{
			stringBuilder.Append($"{name}:{JsonConvert.SerializeObject(value)}|");
		}

		public List<SerializedField> ReadToEnd()
		{
			if (string.IsNullOrWhiteSpace(objectStream))
				return new List<SerializedField>();

			List<SerializedField> stream = new List<SerializedField>();
			//string[] streamData = objectStream.Split("|");
			//foreach(string open in streamData)
			//{
			//	string[] field = open.Split(':');
			//	if (field.Length < 2)
			//		continue;
				
			//	string name = field[0];
			//	StringBuilder sb = new StringBuilder();
			//	for(int i = 1; i < field.Length; i++)
			//		sb.Append(field[i] + (i < field.Length - 1 ? ":" : ""));

			//	string value = sb.ToString();
			//	SerializedField serialization = new SerializedField(name, value);
			//	stream.Add(serialization);
			//}
			return stream;
		}

		public void Open()
		{
			objectStream = "";
			stringBuilder = new StringBuilder();
			open = true;
		}

		public void Close()
		{
			objectStream = stringBuilder.ToString().Trim('|');
			stringBuilder = null;
			open = false;
		}

		public override string ToString() => $"{{ {stringBuilder.ToString().TrimEnd('|')} }}";

	}
}