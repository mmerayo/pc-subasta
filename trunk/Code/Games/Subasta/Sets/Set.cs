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

			Deal(deck);

		}

		private void Deal(IDeck deck)
		{
			throw new NotImplementedException();
		}

		public event SetEventHandler OnCompleted;
		protected virtual void FireCompleted()
		{
			if (OnCompleted != null)
			{
				//provide the deck after getting the hand to the suscriber 
				OnCompleted(this);
			}
		}
	}
}
