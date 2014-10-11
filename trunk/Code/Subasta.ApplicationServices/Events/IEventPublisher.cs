using Subasta.Domain.Events;

namespace Subasta.ApplicationServices.Events
{
	public interface IEventPublisher
	{
		void Publish<TEventData>(TEventData data) where TEventData : IAppEvent;
		void PublishSync<TEventData>(TEventData data) where TEventData : IAppEvent;
	}
}