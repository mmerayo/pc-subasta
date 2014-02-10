using System;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.DomainServices;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.DomainServices
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
					if (trump.Name != "Oros") //para que se evalue como las 40
						result = playerCards.Count(x => x.Suit == Suit.FromName("Oros") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaCopas:
                    if (trump.Name != "Copas")//para que se evalue como las 40
						result = playerCards.Count(x => x.Suit == Suit.FromName("Copas") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaEspadas:
                    if (trump.Name != "Espadas")//para que se evalue como las 40
						result = playerCards.Count(x => x.Suit == Suit.FromName("Espadas") && (x.Number == 11 || x.Number == 12)) == 2;
					break;
				case Declaration.ParejaBastos:
                    if (trump.Name != "Bastos")//para que se evalue como las 40
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