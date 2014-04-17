using Subasta.Domain.Deck;

namespace Subasta.Domain.Game
{
	public interface IFigure
	{
		SayKind  Say { get; }
		SayKind AlternativeSay { get; }
		int PointsBet { get; }
		int AlternativePointsBet { get; }
		ICard[] MarkedCards { get; }
		bool CanBeRepeated { get; }
		bool IsAvailable(ISaysStatus saysStatus, int normalizedTopPoints);
		void MarkFigures(ISaysStatus saysStatus);
		void UnMarkPotentialCandidates();
	}
}