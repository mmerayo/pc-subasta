using System;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataAllocator
	{
		Guid CreateNewGame();
		void RecordGenerationOutput(bool successful);
	}
}
