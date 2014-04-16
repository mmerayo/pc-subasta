using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;

namespace Subasta.DomainServices.Game.Algorithms.Figures
{
	internal class FigurePaso : IFigure
	{
		public SayKind Say
		{
			get { return SayKind.Paso;}
		}

		public SayKind AlternativeSay { get { return Say; } }

		public int PointsBet
		{
			get { return 0; }
		}

		public ICard[] MarkedCards { get{ return new ICard[0];} }

		public bool IsAvailable(ISaysStatus saysStatus, int normalizedPoints)
		{
			return !saysStatus.Says.Any(x => x.PlayerNum == saysStatus.Turn && x.Figure.Say == Say);
		}

		public void MarkFigures(ISaysStatus saysStatus)
		{
			//DOES NOT MARK ANYTHING
		}

		public void UnMarkPotentialCandidates()
		{
			//DOES NOT MARK ANYTHING
		}
	}
}