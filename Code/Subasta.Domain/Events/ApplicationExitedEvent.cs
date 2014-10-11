using System;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class ApplicationExitedEvent : ApplicationEvent
	{
		private ApplicationExitedEvent()
		{
		}

		public static ApplicationExitedEvent Create()
		{
			return new ApplicationExitedEvent();
		}
	}
}