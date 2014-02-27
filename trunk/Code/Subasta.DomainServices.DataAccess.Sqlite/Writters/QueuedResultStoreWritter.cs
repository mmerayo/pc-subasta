using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Subasta.Domain.Game;
using Subasta.Infrastructure.ApplicationServices.Queues;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
	class QueuedResultStoreWritter : ProducerParallelConsumerQueue<NodeResult>, IQueuedResultStoreWritter
	{
		private readonly IResultStoreWritter _resultStoreWritter;



		public QueuedResultStoreWritter(IResultStoreWritter resultStoreWritter) : base(1, 8, 20, TimeSpan.FromMinutes(1))
		{
			_resultStoreWritter = resultStoreWritter;
		}

		protected override Func<NodeResult, bool> RunActionOnDequeue
		{
			get
			{
				return OnQueueRead;
			}
		}

		private bool OnQueueRead(NodeResult arg)
		{
			try
			{
				_resultStoreWritter.Add(arg);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
			Console.WriteLine("Saved Item");
			return true;
		}

		public void Add(NodeResult result)
		{
			this.EnqueueItem(result);
		}

		public void Add(IEnumerable<NodeResult> result)
		{
			result.ForEach(EnqueueItem);
		}

	}
}
