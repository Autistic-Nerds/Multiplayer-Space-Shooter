using System.Data;

namespace Cosmos.SQLite
{
	public interface IDatabaseProvider
	{
		IDbConnection CreateConnection();
	}
}