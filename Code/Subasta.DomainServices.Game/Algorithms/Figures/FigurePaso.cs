using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
    internal class FigurePaso : IFigure
    {
        public SayKind Say
        {
            get { return SayKind.Paso; }
        }

        public bool IsAvailable(ISaysStatus saysStatus, int topPoints)
        {
            return saysStatus.Says
        }
            
      }
}