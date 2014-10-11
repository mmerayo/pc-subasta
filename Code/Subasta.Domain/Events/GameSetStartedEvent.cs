using System;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class GameSetStartedEvent : ApplicationEvent
	{
		private GameSetStartedEvent()
		{
		}

		public static GameSetStartedEvent Create()
		{
			return new GameSetStartedEvent();
		}
	}
}