namespace Games.Subasta.Sets
{
	public delegate void SetEventHandler(ISet set);
	public interface ISet
	{
		void Start();

		event SetEventHandler OnCompleted;
	}
}