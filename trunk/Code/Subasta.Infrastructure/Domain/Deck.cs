using System;
using System.Linq;
using Subasta.Domain.Deck;

namespace Subasta.Infrastructure.Domain
{
	sealed class Deck : IDeck
	{
		public IPack Cards { get; private set; }

		public static IDeck DefaultForSubasta
		{
			get
			{
				var cards = new ICard[40];
				var idx = 0;
				for (var number = 1; number <= 12; number++)
				{
					foreach (var suit in Suit.Suits)
					{

						if(number!=8 && number!=9)
						{
							cards[idx++] = new Card(suit,number);
						}
					}
				}
				return new Deck(new Pack(cards));
			}
		}

		public void SetCards(IPack cards)
		{
			Cards=cards;
		}

		public ICard Get(int number, string suitName)
		{
			return
				Cards.Cards
				     .SingleOrDefault(
					     x =>
					     x.Number == number && string.Compare(x.Suit.Name, suitName, StringComparison.InvariantCultureIgnoreCase) == 0);
		}

		private Deck(IPack cards)
		{
			Cards = cards;
		}
	}
}