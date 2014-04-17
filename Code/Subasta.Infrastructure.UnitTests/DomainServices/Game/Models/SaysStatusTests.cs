using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.DomainServices.Game.Models;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Models
{
	[TestFixture]
	class SaysStatusTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[TestCaseSource("Can_Get_PointsBet_CaseSources")]
		public int Can_Get_PointsBet(List<Tuple<int,IFigure>> says)
		{
			_context.WithSays(says);

			return _context.Sut.PointsBet;
		}

		public static IEnumerable Can_Get_PointsBet_CaseSources()
		{
			//without repeatable
			var says = new List<Tuple<int,IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigurePaso()),
				new Tuple<int,IFigure>(2,new FigureTreses()),
				new Tuple<int,IFigure>(3,new FigurePaloCorrido())
			};
			yield return new TestCaseData(says).Returns(9);
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigurePaso()),
				new Tuple<int,IFigure>(2,new FigureTreses())
			};
			yield return new TestCaseData(says).Returns(7);
			//with all repeatables
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				
			};
			yield return new TestCaseData(says).Returns(2);

			//with all repeatables adding alternate points
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				new Tuple<int,IFigure>(3,new FigureParejaSegura())
			};
			yield return new TestCaseData(says).Returns(3);

			//with some repeatables

			//with repeatables but last not repeatable
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				new Tuple<int,IFigure>(3,new FigurePaloCorrido())
			};
			yield return new TestCaseData(says).Returns(9);
		}

		private class TestContext
		{
			IFixture _fixture=new Fixture().Customize(new SubastaAutoFixtureCustomizations());
			private SaysStatus _sut;
			private IExplorationStatus _explorationStatus;
			public TestContext()
			{

				_fixture.Register(() => 1);
				_explorationStatus=_fixture.Freeze<IExplorationStatus>();
				_explorationStatus.Stub(x => x.Clone()).Return(_explorationStatus);

				_explorationStatus.Stub(x => x.PlayerCards(Arg<int>.Is.Anything)).Return(new[] {new Card("O1")});
			}

			public SaysStatus Sut
			{
				get { return _sut ?? (_sut = _fixture.Create<SaysStatus>()); }
			}

			public TestContext WithSays(IEnumerable<Tuple<int, IFigure>> says)
			{
				foreach (var tuple in says)
					Sut.Add(tuple.Item1,tuple.Item2);
				return this;
			}
		}
	}
}
