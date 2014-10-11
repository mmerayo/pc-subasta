using Subasta.ApplicationServices.IO;

namespace Subasta.Infrastructure.ApplicationServices.Events
{
	internal class NullEventsPublisher : EmailEventsPublisher
	{
		public NullEventsPublisher(IPathHelper pathHelper) : base(pathHelper)
		{
		}

		protected override void SendEvent(string fileName)
		{
			//TODO: remove
			//base.SendEvent(fileName);
			//do nothing
		}
	}
}