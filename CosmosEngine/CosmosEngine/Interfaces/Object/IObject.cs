//Written by Philip Wittusen
namespace CosmosEngine.CoreModule
{
	public interface IObject
	{
		bool Enabled { get; set; }
		bool Expired { get; }
		bool DestroyOnLoad { get; }
	}
}