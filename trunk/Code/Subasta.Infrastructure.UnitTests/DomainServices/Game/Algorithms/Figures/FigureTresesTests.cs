using System.Collections;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigureTresesTests : FiguresTests<FigureTreses>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigureTreses>.CanPerform_IsAvailable_TestCases();//issue with nunit
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return new ISayCard[]
			{
				new SayCard("O3"),
				containingTheFigure?new SayCard("E3"):new SayCard("O10"),
				
			};
		}
	}
}