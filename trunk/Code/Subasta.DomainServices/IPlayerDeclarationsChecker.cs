using Games.Subasta.GameGeneration.AI;

namespace Games.Subasta
{
	internal interface IPlayerDeclarationsChecker
	{
		bool HasDeclarable(Declaration declarable, ISuit trump, ICard[] playerCards);
	}
}