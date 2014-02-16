using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess;

namespace Subasta.Infrastructure.DomainServices.DataAccess
{
	class GameDataAllocator:IGameDataWritter
	{
		private readonly IGameDataHelper _dbHelper;

		public GameDataAllocator(IGameDataHelper dbHelper)
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
            _dbHelper.ReleaseResources(gameId);

			throw new NotImplementedException();
		}

       
	}
}
