using System;

namespace Subasta.DomainServices.Game
{
	public interface IGameGenerator
	{
		Guid GenerateNewGame();
	}
}
