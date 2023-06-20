using Cosmos.SQLite;

namespace SpaceBattle.Scripts.Unit
{
	internal class SpaceShooter : IRepositoryElement
	{
		[SerialisedValue] private float value;
		[SerialisedValue] private string name;

		public int ID { get; set; }
	}

	public class DataBase
	{
		private void Start()
		{
			Repository.Create<SpaceShooter>("shooters");
		}
	}
}