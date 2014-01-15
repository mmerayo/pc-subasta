using System;
using System.Collections.Generic;
using Games.Deck;
using Games.Subasta.Sets;

namespace Games.Subasta
{
	class Game
	{
		private readonly ISetFactory _setFactory;
		private IPlayer[] _players;
		private List<ISet> _sets;
		private int _dealerPosition;

		public Game(ISetFactory setFactory)
		{
			_setFactory = setFactory;
		}

		public void StartNew(GameConfiguration configuration)
		{
			if(!configuration.IsValid())
				throw new InvalidOperationException("configuration is not valid");
			
			_players=new IPlayer[4];
			Array.Copy(configuration.Players,_players,4);
			_sets=new List<ISet>();
			_dealerPosition = configuration.InitialDealer-1;
			CreateNewSet(new Deck());
		}

		private void CreateNewSet(IDeck deck)
		{
			var set = _setFactory.CreateNew();
			set.OnCompleted += new SetEventHandler(set_OnCompleted);
			_sets.Add(set);
			set.Run(deck,_players,_dealerPosition);
		}

		void set_OnCompleted(ISet set)
		{
			SetNextDealer();
		}

		private void SetNextDealer()
		{
			if (++_dealerPosition == 4) _dealerPosition = 0;
		}

		public IPlayer[] Players
		{
			get { return _players; }
		}

		public List<ISet> Sets
		{
			get { return _sets; }
		}

		public IPlayer CurrentDealer
		{
			get { return Players[_dealerPosition]; }
		}
	}
}
