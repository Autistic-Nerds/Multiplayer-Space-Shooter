
namespace Cosmos
{
	[System.Flags]
	public enum ReadFileOption
	{
		None = 0,
		Distinct = 1 << 0,
		KeepEmpty = 1 << 1,
	}
}