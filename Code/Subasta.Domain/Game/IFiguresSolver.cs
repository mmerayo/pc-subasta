namespace Subasta.Domain.Game
{
    public interface IFiguresSolver{
        IFigure GetFigure(ISaysStatus saysStatus);
    }

    public interface IFigure
    {
        SayKind  Say { get; }
		SayKind AlternativeSay { get; }
    	int PointsBet { get; }
    	bool IsAvailable(ISaysStatus saysStatus, int normalizedPoints);
	    void MarkFigures(ISaysStatus saysStatus);
	    void UnMarkPotentialCandidates();
    }
}