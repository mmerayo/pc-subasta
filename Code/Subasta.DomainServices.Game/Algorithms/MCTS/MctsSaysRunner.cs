using System;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.MCTS
{
	internal class MctsSaysRunner : IMctsSaysRunner, IDisposable
	{
		private const int ROOT_OROS = 0;
		private const int ROOT_COPAS = 1;
		private const int ROOT_ESPADAS = 2;
		private const int ROOT_BASTOS = 3;

		private TreeNode[] _roots;

		public void Start(ISaysStatus sourceStatus)
		{
		//TODO:
		}

		public void Reset()
			{//TODO:
		}

		public SayKind GetSay(ISaysStatus saysStatus)
		{
			return SayKind.As;//TODO: THIS AND THE INTERPRETER
		}

		public void Dispose()
			{//TODO:
		}
	}
}