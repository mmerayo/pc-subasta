using System.Windows.Forms;
using Subasta.DomainServices;

namespace Subasta
{
	class ApplicationEventsExecutorExecutor : IApplicationEventsExecutor
	{
		public void Execute()
		{
			Application.DoEvents();
		}
	}
}