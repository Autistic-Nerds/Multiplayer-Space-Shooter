using System.Collections.Generic;
using System.Data;

namespace Cosmos.SQLite
{
	public interface IMapper<T> where T : IRepositoryElement
	{
		List<T> MapDataReaderToList(IDataReader reader);
	}
}