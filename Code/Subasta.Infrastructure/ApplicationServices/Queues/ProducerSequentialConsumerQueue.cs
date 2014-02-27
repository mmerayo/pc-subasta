using Subasta.ApplicationServices.Queues;

namespace Subasta.Infrastructure.ApplicationServices.Queues
{
	internal abstract class ProducerSequentialConsumerQueue<TQueueItem> : ProducerParallelConsumerQueue<TQueueItem>
	{
		protected ProducerSequentialConsumerQueue()
			: base(1, 1)
		{ }
	}
}