using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.DomainServices.DataAccess.Db
{
	public interface IDbHelper
	{
		void CreateDatabase(Guid gameId);
		void DropDatabase();
		string GetConnectionString();
	}
}
