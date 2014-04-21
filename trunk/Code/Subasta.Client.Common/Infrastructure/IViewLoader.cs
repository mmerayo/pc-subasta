namespace Subasta.Client.Common.Infrastructure
{
	public enum Views
	{
		GameView=1,
		SaysView
	}

	public interface IViewLoader
	{
		void ShowView(Views view);
		void HideView(Views view);
	}
}
