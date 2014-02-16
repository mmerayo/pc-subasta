using System;

namespace Subasta.DomainServices.DataAccess
{
	public interface IDbHelper
	{
		void CreateDatabase(Guid gameId);
		void DropDatabase();
		string GetConnectionString();
	}
}
