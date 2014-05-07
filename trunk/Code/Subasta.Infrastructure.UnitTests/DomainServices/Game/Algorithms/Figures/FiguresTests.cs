using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	[TestFixture]
	internal abstract class FiguresTests<TFigure> where TFigure : class, IFigure
	{
		private TestContext _context;

		[SetUp]
		protected virtual void OnSetUp()
		{
			_context = new TestContext(GetCards(true), GetCards(false));
		}

		protected abstract ISayCard[] GetCards(bool containingTheFigure);

		[Test,TestCaseSource( "CanPerform_IsAvailable_TestCases")]
		public bool CanPerform_IsAvailable(bool hasTheFigure,int topPointsLimit,bool alreadyMarked)
		{
			_context
				.WithEmptySayStatus();
			if (alreadyMarked)
			{
				_context.WithAlreadyMarked();
			}
			else
			{
				_context.WithPlayerHavingFigure(hasTheFigure);
			}
			bool result = _context.Sut.IsAvailable(_context.Status,(byte) topPointsLimit);
			_context.VerifyExpectations();

			return result;
		}


		public static IEnumerable CanPerform_IsAvailable_TestCases()
		{
			//having it
			yield return new TestCaseData(true, 13,false).Returns(true);//top points ok
			yield return new TestCaseData(true, 0, false).Returns(false);//toppoints reached

			//not having it
			yield return new TestCaseData(false, 13, false).Returns(false);//top points ok

			//it was marked
			yield return new TestCaseData(true, 13, true).Returns(false);//top points ok

		}

		internal sealed class TestContext
		{
			private readonly ISayCard[] _playerCardsContainingTheFigure;
			private readonly ISayCard[] _playerCardsNotContainingTheFigure;
			private readonly IFixture _fixture = new Fixture().Customize(new SubastaAutoFixtureCustomizations());
			private TFigure _sut;
			private readonly ISaysStatus _saysStatus;

			public TestContext(ISayCard[] playerCardsContainingTheFigure, ISayCard[] playerCardsNotContainingTheFigure)
			{
				_playerCardsContainingTheFigure = playerCardsContainingTheFigure;
				_playerCardsNotContainingTheFigure = playerCardsNotContainingTheFigure;
				_saysStatus = _fixture.Freeze<ISaysStatus>();
				_saysStatus.Stub(x => x.Turn).Return(1);
			}



			protected IFixture Fixture
			{
				get { return _fixture; }
			}

			public TFigure Sut
			{
				get { return _sut ?? (_sut = Fixture.Create<TFigure>()); }
			}

			public ISaysStatus Status
			{
				get { return _saysStatus; }
			}

			public TestContext WithEmptySayStatus()
			{
				_saysStatus.Stub(x => x.Says).Return(new List<ISay>());

				return this;
			}

			public TestContext WithPlayerHavingFigure(bool containingTheFigure)
			{
				int playerNum = _saysStatus.Turn;
				ISayCard[] objToReturn = containingTheFigure ? _playerCardsContainingTheFigure : _playerCardsNotContainingTheFigure;
				_saysStatus
					.Expect(x => x.GetPlayerCards(playerNum))
					.Return(objToReturn);

				return this;
			}

			public void VerifyExpectations()
			{
				_saysStatus.VerifyAllExpectations();
			}

			public TestContext WithAlreadyMarked()
			{
				WithPlayerHavingFigure(true);
				MarkFigureCards();

				return this;
			}

			private void MarkFigureCards()
			{
				foreach (var sayCard in _playerCardsContainingTheFigure)
				{
					sayCard.Marked = true;
				}
			}
		}
	}
}
