using System.Collections;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigureParejaNoSeguraTests : FiguresTests<FigureParejaNoSegura>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigureParejaNoSegura>.CanPerform_IsAvailable_TestCases();//issue with nunit
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return new ISayCard[]
			{
				new SayCard("O7"),
				containingTheFigure?new SayCard("O6"):new SayCard("O10"),
				new SayCard("O11"),
				new SayCard("O12")
				
			};
		}
	}
}