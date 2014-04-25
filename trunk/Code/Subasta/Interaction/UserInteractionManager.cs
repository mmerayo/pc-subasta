using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Subasta.Interaction
{
	class UserInteractionManager:IUserInteractionManager
	{
		private readonly EventWaitHandle _semGame;
		private readonly EventWaitHandle _semPlayer;

		public UserInteractionManager()
		{
			_semGame = new AutoResetEvent(false);
			_semPlayer = new AutoResetEvent(false);
		}

		public void Reset()
		{
			_semGame.Reset();
			_semPlayer.Reset();
		}

		public void WaitUserInput()
		{
			_semPlayer.Set();
			if (!_semGame.WaitOne())
				throw new Exception();
		}

		public void InputProvided(Action action)
		{
			if (!_semPlayer.WaitOne())
				throw new Exception();
			action();
			_semGame.Set();
		}
	}
}
