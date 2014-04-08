using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game
{
    public interface IFiguresSolver{
        SayKind GetSay(ISaysStatus saysStatus);
    }
}