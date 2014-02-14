using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.DataAccess.Db;

namespace Subasta.Infrastructure.DomainServices.DataAccess
{
	class GameDataAllocator:IGameDataAllocator
	{
		private readonly IDbHelper _dbHelper;

		public GameDataAllocator(IDbHelper dbHelper)
		{
			_dbHelper = dbHelper;
		}

		public Guid CreateNewGameStorage()
		{
			Guid result = Guid.NewGuid();

			_dbHelper.CreateDatabase(result);

			return result;
		}

		public void RecordGenerationOutput(Guid gameId, bool successful)
		{
			throw new NotImplementedException();
		}
	}
}
