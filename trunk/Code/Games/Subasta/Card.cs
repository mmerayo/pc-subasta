using System;
using Subasta.Deck;

namespace Subasta.Subasta
{
	class Card : Deck.Card
	{
		public Card(Suit suit, int number)
			: base(suit,number)
		{
			Value = GetValue(Number);
		}

		public int Value { get; private set; }


		private int GetValue(int number)
		{
			int result;
			switch (number)
			{
				case 1:
					result = 11;
					break;
				case 3:
					result = 10;
					break;
				case 10:
					result = 2;
					break;
				case 11:
					result = 3;
					break;
				case 12:
					result = 4;
					break;

				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
					result = 0;
					break;
				default:
					throw new InvalidOperationException();
			}

			return result;
		}

	}
}