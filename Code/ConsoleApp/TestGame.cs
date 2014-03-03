using Subasta.Domain.Deck;
using Subasta.DomainServices.Game;

namespace ConsoleApp
{
	public class TestGameSimulator:IGameSimulator
	{
		private readonly IDeck _deck;
		private readonly IDeckSuffler _suffler;

		public TestGameSimulator(IDeck deck,
		                IDeckSuffler suffler)
		{
			_deck = deck;
			_suffler = suffler;
		}

		public void Start()
		{
			throw new System.NotImplementedException();
		}

		public bool IsFinished { get; set; }
		public void NextMove()
		{
			throw new System.NotImplementedException();
		}
	}
}