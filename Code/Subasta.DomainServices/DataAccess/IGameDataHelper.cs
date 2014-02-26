using System;
using System.Data;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataHelper
	{
		void CreateDatabase(Guid gameId);
		void DropDatabase();
		string GetConnectionString();
        IUnitOfWork<TSession> GetUnitOfWork<TSession>(Guid gameId);
	    void ReleaseResources(Guid gameId);
	}
}
