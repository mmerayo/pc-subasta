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
		bool IsAvailable(ISaysStatus saysStatus, int normalizedPoints);
		void MarkFigures(ISaysStatus saysStatus);
		void UnMarkPotentialCandidates();
	}
}