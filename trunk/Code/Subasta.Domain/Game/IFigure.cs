using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IFigure
	{
		SayKind  Say { get; }
		int PointsBet { get; }
		ICard[] MarkedCards { get; }
		bool UsingAlternative { get; }
		bool IsAvailable(ISaysStatus saysStatus, int normalizedTopPoints);
		void MarkFigures(ISaysStatus saysStatus);
		void UnMarkPotentialCandidates();
	}
}