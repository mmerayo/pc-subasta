using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using NHibernate.Linq;
using Subasta.Domain.Game;
using Subasta.Infrastructure.ApplicationServices.Queues;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
	class QueuedResultStoreWritter : ProducerParallelConsumerQueue<NodeResult>, IQueuedResultStoreWritter
	{
		private readonly IResultStoreWritter _resultStoreWritter;

		private BackgroundWorker _writterWorker=new BackgroundWorker();

		private readonly List<NodeResult> _pendingItems=new List<NodeResult>();
		private readonly System.Timers.Timer _timer;

		private readonly object _syncLock=new object();
		public QueuedResultStoreWritter(IResultStoreWritter resultStoreWritter) : base(1, 8, 20, TimeSpan.FromMinutes(1))
		{
			_resultStoreWritter = resultStoreWritter;
			_writterWorker.DoWork += _writterWorker_DoWork;
			_timer = new System.Timers.Timer(5000);
			_timer.Enabled = true;
			_timer.AutoReset = true;
			_timer.Elapsed += _timer_Elapsed;

			//_timer.Start();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_timer.Stop();
				_timer.Dispose();
			}
			
			base.Dispose(disposing);
		}

		void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Console.WriteLine("_timer_Elapsed, pending items:{0}",_pendingItems.Count);
			RunWorker();
		}

		private void RunWorker()
		{
			if (_writterWorker.IsBusy != true)
			{
				_writterWorker.RunWorkerAsync();
			}
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
				lock (_syncLock)
				{
					_pendingItems.Add(arg);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
			
			return true;
		}

		void _writterWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine("_writterWorker_DoWork:Start");

			List<NodeResult> toSave;
			lock (_syncLock)
			{
				toSave=new List<NodeResult>(_pendingItems);
				_pendingItems.Clear();
			}
			try
			{
				_resultStoreWritter.Add(toSave);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excetion!!: {0}",ex);
				throw;
			}
			Console.WriteLine("Saved {0} Items", toSave.Count);

			Console.WriteLine("_writterWorker_DoWork:END");
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
