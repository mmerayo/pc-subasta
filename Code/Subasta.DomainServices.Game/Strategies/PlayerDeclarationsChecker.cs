﻿using System;
using System.Linq;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.Game.Strategies
{
	internal sealed class PlayerDeclarationsChecker : IPlayerDeclarationsChecker
	{
		private readonly ISuit _suitOros = Suit.FromName("Oros");
		private readonly ISuit _suitCopas = Suit.FromName("Copas");
		private readonly ISuit _suitEspadas=Suit.FromName("Espadas");
		private readonly ISuit _suitBastos = Suit.FromName("Bastos");


		public bool HasDeclarable(Declaration declarable, ISuit trump, ICard[] playerCards)
		{
			bool result = false;
			switch (declarable)
			{
				case Declaration.Reyes:
					result = playerCards.Count(x => x.Number == 12) == 4;
					break;
				case Declaration.Caballos:
					result = playerCards.Count(x => x.Number == 11) == 4;
					break;
				case Declaration.ParejaOros:
					if (trump!= _suitOros) //para que se evalue como las 40
					{
						result = playerCards.Count(x => x.Suit == _suitOros && (x.Number == 11 || x.Number == 12)) == 2;
					}
					break;
				case Declaration.ParejaCopas:
					if (trump!= _suitCopas) //para que se evalue como las 40
					{
						result = playerCards.Count(x => x.Suit == _suitCopas && (x.Number == 11 || x.Number == 12)) == 2;
					}
					break;
				case Declaration.ParejaEspadas:
					if (trump!= _suitEspadas) //para que se evalue como las 40
					{
						result = playerCards.Count(x => x.Suit == _suitEspadas && (x.Number == 11 || x.Number == 12)) == 2;
					}
					break;
				case Declaration.ParejaBastos:
					if (trump != _suitBastos) //para que se evalue como las 40
					{
						result = playerCards.Count(x => x.Suit == _suitBastos && (x.Number == 11 || x.Number == 12)) == 2;
					}
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