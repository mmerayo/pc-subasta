using System;
using Games.Deck;

namespace Games.Subasta.Sets
{
	class Set:ISet
	{
		private readonly ISuffleStrategy _suffler;
		private IPlayer[] _players;
		public Set(ISuffleStrategy suffler)
		{
			_suffler = suffler;
		}

		public void Run(IDeck deck,IPlayer[] players,int dealerPosition)
		{
			if(dealerPosition<1 || dealerPosition>4)
				throw new ArgumentOutOfRangeException("dealerPosition");
			_players = players;

			_suffler.Suffle(ref deck);

			//REPARTIR

		}

		public event SetEventHandler OnCompleted;
		protected virtual void FireCompleted()
		{
			if (OnCompleted != null)
			{
				OnCompleted(this);
			}
		}
	}
}
