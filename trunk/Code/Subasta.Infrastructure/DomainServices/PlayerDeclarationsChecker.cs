using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Subasta.GameGeneration.AI;

namespace Games.Subasta
{
	internal sealed class PlayerDeclarationsChecker : IPlayerDeclarationsChecker
	{
		public bool HasDeclarable(Declaration declarable, ISuit trump, ICard[] playerCards)
		{
			bool result=false;
			switch (declarable)
			{
				case Declaration.Reyes:
					result = playerCards.Count(x => x.Number == 12) == 4;
					break;
				case Declaration.Caballos:
					result = playerCards.Count(x => x.Number == 11) == 4;
					break;
				case Declaration.ParejaOros:
					if (trump.Name != "Oros")
						result = playerCards.Count(x => x.Suit == Suit.FromName("Oros") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaCopas:
					if (trump.Name != "Copas")
						result = playerCards.Count(x => x.Suit == Suit.FromName("Copas") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaEspadas:
					if (trump.Name != "Espadas")
						result = playerCards.Count(x => x.Suit == Suit.FromName("Espadas") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaBastos:
					if (trump.Name != "Bastos")
						result = playerCards.Count(x => x.Suit == Suit.FromName("Bastos") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.Cuarenta:
					result = playerCards.Count(x => x.Suit == trump && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				default:
					throw new ArgumentOutOfRangeException("declarable");
			}

			return result;
		}
	}
}