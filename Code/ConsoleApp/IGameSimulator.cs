namespace ConsoleApp
{
	public interface IGameSimulator
	{
		void Start();
		bool IsFinished { get; set; }
		void NextMove();
		event StatusChangedHandler GameStatusChanged;
	}
}