using System;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class GameSetCompletedEvent : ApplicationEvent
	{
		private GameSetCompletedEvent()
		{
		}

		public static GameSetCompletedEvent Create()
		{
			return new GameSetCompletedEvent();
		}
	}
}