namespace Subasta.Domain.Game
{
    public interface IFiguresSolver{
        IFigure GetFigure(ISaysStatus saysStatus);
    }
}