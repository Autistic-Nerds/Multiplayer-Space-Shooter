using CosmosEngine.Netcode.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace CosmosEngine.Netcode.Serialization
{
	public sealed class NetcodeWriter : NetcodeStream
	{
		private StringBuilder stringBuilder;

		public override int Size => Encoding.UTF8.GetByteCount(stringBuilder.ToString());

		public override void Open()
		{
			base.Open();
			stringBuilder = new StringBuilder();
		}

		public override void Close()
		{
			base.Close();
			Stream = stringBuilder.ToString();
			stringBuilder.Clear();
		}

		public void Write(object value)
		{
			WriteData(Serialize(value));
		}

		internal void WriteSyncVar(string name, object value)
		{
			WriteData($"{Serialize(name)}:{Serialize(value)}");
		}

		internal void FinializeSyncVar()
		{
			stringBuilder.Append($"||");
		}

		private void WriteData(string data)
		{
			if (!IsOpen)
			{
				throw new System.Exception($"Unable to write to {GetType().FullName} before Open() has been invoked.");
			}

			//Using | to split.
			//data.Replace('|', '/');
			//if (StringBuilder.Length > 0)
			//	data = data.Insert(0, "|");
			//StringBuilder.Append(data);

			//Using { } to split.
			stringBuilder.Append($"{{{data}}}");
		}

		private string Serialize(object value)
		{
			return JsonConvert.SerializeObject(value);
		}

		public override string ToString() => $"{{{stringBuilder.ToString()}}}";
	}
}