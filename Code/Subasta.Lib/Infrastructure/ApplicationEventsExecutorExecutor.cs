using System.Windows.Forms;
using Subasta.DomainServices;

namespace Subasta.Lib.Infrastructure
{
	class ApplicationEventsExecutorExecutor : IApplicationEventsExecutor
	{
		public void Execute()
		{
			Application.DoEvents();
		}
	}
}