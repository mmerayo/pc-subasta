using System;
using System.Xml.Serialization;

namespace Subasta.Domain.Events
{
	[Serializable]
	public class ApplicationStartedEvent : ApplicationEvent
	{
		private ApplicationStartedEvent()
		{
		}

		public static ApplicationStartedEvent Create()
		{
			return new ApplicationStartedEvent();
		}
	}
}