using CosmosEngine.NetCode.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace NetCode.Serialization
{
	public class NetCodeStream
	{
		private string stream;
		public string Stream { get => stream; internal set => stream = value; }
	}
}