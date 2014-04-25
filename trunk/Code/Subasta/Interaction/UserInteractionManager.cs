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
		private object _lastInput = null;
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

		public TInput WaitUserInput<TInput>()
		{
			_semPlayer.Set();
			if (!_semGame.WaitOne())
				throw new Exception();

			var result=  (TInput) _lastInput;
			_lastInput = null;
			return result;
		}

		public void InputProvided<TInput>(Func<TInput> action)
		{
			if (!_semPlayer.WaitOne())
				throw new Exception();
			_lastInput=action();
			_semGame.Set();
		}
	}
}
