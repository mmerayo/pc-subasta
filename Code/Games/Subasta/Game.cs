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
		private IPlayer[] _players;
		private List<ISet> _sets;

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
			CreateNewSet();
		}

		private void CreateNewSet()
		{
			var set = _setFactory.CreateNew();
			_sets.Add(set);
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
	}
}
