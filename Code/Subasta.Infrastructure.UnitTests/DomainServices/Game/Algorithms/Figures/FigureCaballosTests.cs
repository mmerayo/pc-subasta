using System.Collections;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.Infrastructure.Domain;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	class FigureCaballosTests : FiguresTests<FigureCaballos>
	{
		public new static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			return FiguresTests<FigureCaballos>.CanPerform_IsAvailable_TestCases();//issue with nunit
		}

		protected override ISayCard[] GetCards(bool containingTheFigure)
		{
			return containingTheFigure
				? new ISayCard[]
				{
					new SayCard("O11"),
					new SayCard("C11"),
					new SayCard("E11"),
					new SayCard("B11"),

				}
				: new ISayCard[] {new SayCard("O1"),};
		}
	}
}