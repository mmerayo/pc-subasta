using System;
using Subasta.Domain.Game;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class GameCompletedEvent : ApplicationEvent
	{
		private GameCompletedEvent(){}
		private GameCompletedEvent(IExplorationStatus status)
		{
			GameId = status.GameId.ToString();
			Winner = status.TeamWinner;
		}

		protected int Winner { get; set; }

		protected string GameId { get; set; }

		public static GameCompletedEvent From(IExplorationStatus status)
		{
			return new GameCompletedEvent(status);
		}
	}
}