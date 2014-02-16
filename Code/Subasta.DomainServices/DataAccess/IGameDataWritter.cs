using System;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.DataAccess
{
	public interface IGameDataWritter
	{
		Guid CreateNewGameStorage();
		void RecordGenerationOutput(Guid gameId, bool successful);
	}
}
