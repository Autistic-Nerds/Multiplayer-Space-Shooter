namespace NetCode.Serialization
{
	public class NetCodeWriter : NetCodeStream
	{
		public void Write()
		{

		}

		public void WriteStream(string stream)
		{
			this.Stream = stream;
		}
	}
}