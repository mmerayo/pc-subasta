using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Subasta.Sets;

namespace Games.Subasta
{
	class Game
	{
		private readonly ISetFactory _setFactory;
		private readonly ISuffleStrategy _suffler;
		private IPlayer[] _players;
		private List<ISet> _sets;
		private int _dealerPosition;

		public Game(ISetFactory setFactory,ISuffleStrategy suffler)
		{
			_setFactory = setFactory;
			_suffler = suffler;
		}

		public void StartNew(GameConfiguration configuration)
		{
			if(!configuration.IsValid())
				throw new InvalidOperationException("configuration is not valid");
			
			_players=new IPlayer[4];
			Array.Copy(configuration.Players,_players,4);
			_sets=new List<ISet>();
			_dealerPosition = configuration.InitialDealer-1;

			CreateNewSet();
		}

		private void CreateNewSet()
		{
			var set = _setFactory.CreateNew();
			_sets.Add(set);
			_suffler.Suffle();
			set.Start();
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
