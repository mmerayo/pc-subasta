using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Games.Subasta
{
	class Game
	{
		public void StartNew(GameConfiguration configuration)
		{
			throw new NotImplementedException();
		}

		public IPlayer[] Players
		{
			get { throw new NotImplementedException(); }
		}
	}

	class GameConfiguration
	{
		public bool IsValid()
		{
			throw new NotImplementedException();
		}

		public void AddPlayer(int position, IPlayer player)
		{
			if(position<1 || position >4)
				throw new ArgumentOutOfRangeException("position");
			throw new NotImplementedException();
		}
	}
}
