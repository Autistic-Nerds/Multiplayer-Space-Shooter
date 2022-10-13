using CosmosEngine.Netcode.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CosmosEngine.Netcode.Serialization
{
	public sealed class NetcodeReader : NetcodeStream
	{
		private readonly List<SerializedField> syncVarData = new List<SerializedField>();
		private readonly List<string> streamData = new List<string>();
		private int index;

		internal NetcodeReader(string stream)
		{
			this.Stream = stream;
		}

		public override void Open()
		{
			base.Open();
			SplitDataStream();
		}

		public override void Close()
		{
			base.Close();
			syncVarData.Clear();
			streamData.Clear();
		}

		public T? Read<T>()
		{
			if (index < streamData.Count)
			{
				string next = streamData[index++];
				return JsonConvert.DeserializeObject<T>(next);
				if (TryParseJson<T>(next, out T value))
				{
					index++;
					return value;
				}
				else
				{
					//Mismatched type...
				}
			}
			return default(T);
		}

		public object? ReadSyncVar(string name, System.Type type)
		{
			int index = syncVarData.FindIndex(item => item.Index.Equals(name));
			if (index >= 0)
			{
				return JsonConvert.DeserializeObject(syncVarData[index].Value, type);
			}
			return null;
		}

		public IEnumerable<SerializedField> ReadSyncVars()
		{
			return syncVarData;
		}

		private void SplitDataStream()
		{
			index = 0;
			//Handle SyncVars
			if (Stream.Contains("||"))
			{
				string[] splitDataStream = Stream.Split("||");
				if (splitDataStream.Length >= 1)
				{
					int i = 0;
					string[] syncVarValues = DataStreamSplit(splitDataStream[0]);
					foreach (string syncVar in syncVarValues)
					{
						string[] field = syncVar.Split(':');
						if (field.Length < 2)
							continue;
						byte index = byte.Parse(field[0]);
						string value = syncVar.Remove(0, index.ToString().Trim('"').Length + 1);

						SerializedField variable = new SerializedField(index, value);
						syncVarData.Add(variable);
						i += variable.ToString().Length;
					}
					Stream = Stream.Remove(0, splitDataStream[0].Length + 2);
				}
			}

			//Handle Stream
			if (!string.IsNullOrWhiteSpace(Stream))
			{
				string[] dataSegments = DataStreamSplit(Stream);
				foreach (string segment in dataSegments)
				{
					if (string.IsNullOrWhiteSpace(segment))
						continue;
					streamData.Add(segment);
				}
			}
		}

		private string[] DataStreamSplit(string stream)
		{
			List<string> vs = new List<string>();
			vs.AddRange(stream.Split('{', '}'));
			vs.RemoveAll(item => string.IsNullOrWhiteSpace(item));
			return vs.ToArray();
		}

		private bool TryParseJson<T>(string value, out T result)
		{
			try
			{
				// Validate missing fields of object
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.MissingMemberHandling = MissingMemberHandling.Error;
				result = JsonConvert.DeserializeObject<T>(value, settings);
				return true;
			}
			catch (System.Exception)
			{
				result = default(T);
				return false;
			}
		}
	}
}