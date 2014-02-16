using System;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataHelper
	{
		void CreateDatabase(Guid gameId);
		void DropDatabase();
		string GetConnectionString();
	    IUnitOfWork<TSession> GetUnitOfWork<TSession>();
	}
}
