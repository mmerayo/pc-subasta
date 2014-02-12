using System;

namespace Subasta.DomainServices.Game
{
	public interface IGameGenerator
	{
		bool TryGenerateNewGame(out Guid gameId);
	}
}
