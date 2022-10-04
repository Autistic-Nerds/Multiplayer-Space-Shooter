using System.Data.SQLite;
using System.Data;

namespace Cosmos.SQLite
{
	/// <summary>
	/// An SQL structured database provider
	/// </summary>
	public class DatabaseProvider : IDatabaseProvider
	{
		private readonly string connection;
		public DatabaseProvider(string connection)
		{
			this.connection = connection;
		}

		public IDbConnection CreateConnection()
		{
			return new SQLiteConnection(connection);
		}
		/// <summary>
		/// Creates a default <see cref="Cosmos.SQLite.DatabaseProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static DatabaseProvider Create() => new DatabaseProvider("Data Source=:memory:;Version=3;New=true");
	}
}