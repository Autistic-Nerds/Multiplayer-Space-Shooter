
namespace CosmosEngine.CoreModule
{
	public interface IObject
	{
		public string Name { get; }
		bool Enabled { get; set; }
		bool Expired { get; }
		bool DestroyOnLoad { get; }
	}
}