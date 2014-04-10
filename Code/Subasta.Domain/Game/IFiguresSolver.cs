namespace Subasta.Domain.Game
{
    public interface IFiguresSolver{
        IFigure GetFigure(ISaysStatus saysStatus);
    }

    public interface IFigure
    {
        SayKind Say { get; }
        bool IsAvailable(ISaysStatus saysStatus, int topPoints);
    }
}