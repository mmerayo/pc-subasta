using Games.Deck;

namespace Games
{
	internal interface ISuffleStrategy
	{
		void Suffle(ref IDeck deck);
	}
}