using System;

namespace Games.Subasta
{
	class GameConfiguration
	{
		private readonly IPlayer[] _players=new IPlayer[4];
		public bool IsValid()
		{
#if DEBUG
			return true; //TODO
#endif
		}

		public void AddPlayer(int position, IPlayer player)
		{
			if(position<1 || position >4)
				throw new ArgumentOutOfRangeException("position");
			Players[position - 1] = player;
		}

		public IPlayer[] Players
		{
			get { return _players; }
		}
	}
}