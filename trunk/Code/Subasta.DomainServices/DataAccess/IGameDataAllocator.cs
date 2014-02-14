using System;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataAllocator
	{
		Guid CreateNewGameStorage();
		void RecordGenerationOutput(Guid gameId, bool successful);
	}
}
