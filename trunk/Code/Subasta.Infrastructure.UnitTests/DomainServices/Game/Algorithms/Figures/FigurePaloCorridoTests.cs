using System.Collections;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigurePaloCorridoTests : FiguresTests<FigurePaloCorrido>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigurePaloCorrido>.CanPerform_IsAvailable_TestCases();//issue with nunit
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return new ISayCard[]
			{
				new SayCard("O1"),
				new SayCard("O3"),
				containingTheFigure?new SayCard("O12"):new SayCard("O10"),
				
			};
		}
	}
}