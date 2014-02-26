namespace Subasta.Infrastructure.ApplicationServices.Queues
{
	internal abstract class ProducerSequentialConsumerQueue<TQueueItem> : ProducerParallelConsumerQueue<TQueueItem>, IProducerSequentialConsumerQueue
	{
		protected ProducerSequentialConsumerQueue()
			: base(1, 1)
		{ }
	}
}