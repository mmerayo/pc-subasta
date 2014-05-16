using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Subasta.Domain.Game;
using Subasta.DomainServices.Game.Algorithms.Figures.Catalog;
using Subasta.DomainServices.Game.Models;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.UnitTests.Tools;
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
		public int Can_Get_PointsBet(int id, List<Tuple<int,IFigure>> says)
		{
			Console.WriteLine("Id:{0}",id);
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
			yield return new TestCaseData(10,says).Returns(9);
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigurePaso()),
				new Tuple<int,IFigure>(2,new FigureTreses())
			};
			yield return new TestCaseData(20,says).Returns(7);
			//with all repeatables
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				
			};
			yield return new TestCaseData(30,says).Returns(2);

			//with all repeatables adding alternate points
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				new Tuple<int,IFigure>(3,new FigureParejaSegura())
			};
			yield return new TestCaseData(40,says).Returns(3);

			//with some repeatables

			//with repeatables but last not repeatable
			says = new List<Tuple<int, IFigure>>
			{
				new Tuple<int,IFigure>(1,new FigureAs()),
				new Tuple<int,IFigure>(2,new FigureAs()),
				new Tuple<int,IFigure>(3,new FigurePaloCorrido())
			};
			yield return new TestCaseData(50,says).Returns(9);
		}

		[TestCaseSource("Can_Get_IsCompleted_CaseSources")]
		public bool Can_Get_IsCompleted(int id, List<Tuple<int, IFigure>> says)
		{
			Console.WriteLine("Id:{0}", id);
			_context.WithSays(says);

			return _context.Sut.IsCompleted;
		}

		public static IEnumerable Can_Get_IsCompleted_CaseSources()
		{
		//incompleta
		var says = new List<Tuple<int, IFigure>>
			           {
			           	new Tuple<int, IFigure>(1, new FigurePaso()),
			           	new Tuple<int, IFigure>(2, new FigurePaso()),
			           	new Tuple<int, IFigure>(3, new FigurePaso())
			           };
			yield return new TestCaseData(5, says).Returns(false);

			//todos pasan
			 says = new List<Tuple<int, IFigure>>
			           {
			           	new Tuple<int, IFigure>(1, new FigurePaso()),
			           	new Tuple<int, IFigure>(2, new FigurePaso()),
			           	new Tuple<int, IFigure>(3, new FigurePaso()),
						new Tuple<int, IFigure>(4, new FigurePaso())
			           };
			yield return new TestCaseData(10, says).Returns(true);

			//todos pasan el ultimo cierra
			 says = new List<Tuple<int, IFigure>>
			           {
			           	new Tuple<int, IFigure>(1, new FigurePaso()),
			           	new Tuple<int, IFigure>(2, new FigurePaso()),
			           	new Tuple<int, IFigure>(3, new FigurePaso()),
						new Tuple<int, IFigure>(4, new FigureJustPoints(11))
			           };
			yield return new TestCaseData(20, says).Returns(true);

			//el ultimo antes de pasar queda
			says = new List<Tuple<int, IFigure>>
			           {
			           	new Tuple<int, IFigure>(1, new FigurePaso()),
			           	new Tuple<int, IFigure>(2, new FigurePaso()),
			           	new Tuple<int, IFigure>(3, new FigureJustPoints(10)),
						new Tuple<int, IFigure>(4, new FigureJustPoints(11)),
						new Tuple<int, IFigure>(3, new FigurePaso())
			           };
			yield return new TestCaseData(30, says).Returns(true);

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

				_explorationStatus.Stub(x => x.PlayerCards(Arg<byte>.Is.Anything)).Return(new[] {new Card("O1")});
			}

			public SaysStatus Sut
			{
				get { return _sut ?? (_sut = _fixture.Create<SaysStatus>()); }
			}

			public TestContext WithSays(IEnumerable<Tuple<int, IFigure>> says)
			{
			
				foreach (var tuple in says)
				{
					tuple.Item2.IsAvailable(Sut, 25);
					Sut.Add((byte)tuple.Item1,tuple.Item2);
				}
				return this;
			}
		}
	}
}
