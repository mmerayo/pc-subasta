using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IFigure
	{
		SayKind  Say { get; }
		byte PointsBet { get; }
		ICard[] MarkedCards { get; }
		bool UsingAlternative { get; }
		bool IsAvailable(ISaysStatus saysStatus, byte normalizedTopPoints);
		void MarkFigures(ISaysStatus saysStatus);
		void UnMarkPotentialCandidates();
	}
}