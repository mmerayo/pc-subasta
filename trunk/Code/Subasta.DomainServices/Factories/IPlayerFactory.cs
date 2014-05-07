using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.DalModels;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Factories
{
	public interface IPlayerFactory
	{
		IPlayer CreatePlayer(byte playerNumber, StoredGameData source);
	}
}
