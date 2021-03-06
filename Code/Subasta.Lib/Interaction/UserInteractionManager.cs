﻿using System;
using System.Threading;
using log4net;
using Subasta.Client.Common.Game;
using Subasta.DomainServices;

namespace Subasta.Lib.Interaction
{
	class UserInteractionManager:IUserInteractionManager
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (UserInteractionManager));
		private readonly IApplicationEventsExecutor _eventsExecutor;
		private readonly EventWaitHandle _semGame;
		private readonly EventWaitHandle _semPlayer;
		private object _lastInput = null;
		public UserInteractionManager(IApplicationEventsExecutor eventsExecutor)
		{
			_eventsExecutor = eventsExecutor;
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
			_eventsExecutor.Execute();
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
			try
			{
				_lastInput = action();
			}
			catch (Exception ex)
			{
				Logger.Fatal("InputProvided",ex);
				//TODO: message to user, send error and close
				throw;

			}
			finally
			{
				_eventsExecutor.Execute();
				_semGame.Set();
			}

		}
	}
}
