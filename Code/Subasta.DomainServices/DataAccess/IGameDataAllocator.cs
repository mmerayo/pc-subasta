using System;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataAllocator
	{
		Guid CreateNewGame();
		void RecordGenerationOutput(Guid gameId, bool successful);
	}
}
