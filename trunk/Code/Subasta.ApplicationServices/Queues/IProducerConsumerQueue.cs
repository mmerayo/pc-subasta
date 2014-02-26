using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.ApplicationServices.Queues
{
	public interface IProducerConsumerQueue<TQueueItem> : IDisposable
	{
		/// <summary>
		/// Number of threads active currently
		/// </summary>
		int CurrentThreadNumber { get; }

		int Count { get; }
		void EnqueueItem(TQueueItem item);

		void Start();//TODO: REMOVE FROM HERE WHEN Istartable is implemented
	}

}
