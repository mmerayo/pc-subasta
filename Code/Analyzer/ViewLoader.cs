using System;
using Subasta.Client.Common.Infrastructure;

namespace Analyzer
{
	internal class ViewLoader:IViewLoader
	{
		private readonly FrmExplorationStatus _explorationStatus;

		public ViewLoader(FrmExplorationStatus explorationStatus)
		{
			_explorationStatus = explorationStatus;
		}

		public void ShowView(Views view)
		{
			switch(view)
			{
				case Views.GameView:
				case Views.SaysView:
					_explorationStatus.Show();
					break;
				default:
					throw new ArgumentOutOfRangeException("view");
			}
		}

		public void HideView(Views view)
		{
		switch (view)
			{
			case Views.GameView:
				_explorationStatus.Hide();
				break;
			case Views.SaysView:
					break;
			default:
				throw new ArgumentOutOfRangeException("view");
			}
		}
	}
}