using System;

namespace Subasta
{
	public class Card
	{
		internal Card(Suit suit, int number)
		{
			Suit = suit;
			Number = number;
			Value = GetValue(Number);
		}

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

		public Suit Suit { get;  private set; }
		public int Number { get; private set; }
		public int Value { get; private set; }
	}
}
