using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Client.Common;

namespace Analyzer
{
	public class Game
	{
		public Game(IPlayer player1, IPlayer player2, IPlayer player3, IPlayer player4)
		{
			Player4 = player4;
			Player3 = player3;
			Player2 = player2;
			Player1 = player1;
		}

		public IPlayer Player1 { get; private set; }
		public IPlayer Player2 { get; private set; }
		public IPlayer Player3 { get; private set; }
		public IPlayer Player4 { get; private set; }

	}
}
