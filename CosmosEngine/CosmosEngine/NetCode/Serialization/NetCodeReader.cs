using CosmosEngine.NetCode.Serialization;
using System.Collections.Generic;
using System.IO;

namespace NetCode.Serialization
{
	public class NetCodeReader : NetCodeStream
	{

		public IEnumerable<SerializedField> ReadToEnd()
		{
			if (string.IsNullOrWhiteSpace(Stream))
				return new List<SerializedField>();
			return null;
		}
	}
}