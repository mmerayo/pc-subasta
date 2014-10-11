using System;

namespace Subasta.Domain.Events
{
	public interface IAppEvent
	{
		string HostName { get; }
		string UserName { get; }

		string IP { get; }
		DateTime DateTimeUtc { get; }
		string EventName { get; }
	}
}