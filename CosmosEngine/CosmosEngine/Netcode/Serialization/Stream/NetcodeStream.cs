using CosmosEngine.Netcode.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CosmosEngine.Netcode.Serialization
{
	public abstract class NetcodeStream
	{
		private bool open = false;
		private string stream = "";

		protected bool IsOpen => open;
		public string Stream { get => stream; protected set => stream = value; }
		public virtual int Size => Encoding.UTF8.GetByteCount(Stream);

		public virtual void Open()
		{
			open = true;
		}

		public virtual void Close()
		{
			open = false;
		}

		public string Export() => Stream;

		public override string ToString() => $"{{ {Stream} }}";
	}
}