using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Deck;

namespace Games.Subasta.AI
{
	public class Status
	{
		public void SetTurn(int playerNum)
		{
			if(playerNum<1 || playerNum>4)
				throw new ArgumentOutOfRangeException("playerNum");

			throw new NotImplementedException();
		}

		public void SetCards(int playerNum, ICard[] cards)
		{
			throw new NotImplementedException();
		}
	}
}
