using System;
using Subasta.Domain.DalModels;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class GameStartedEvent : ApplicationEvent
	{
		public string GameId { get; set; }

		private GameStartedEvent(){}
		private GameStartedEvent(StoredGameData status)
		{
			GameId = status.GameId.ToString();
		}

		public static GameStartedEvent From(StoredGameData status)
		{
			return new GameStartedEvent(status);
		}
	}
}