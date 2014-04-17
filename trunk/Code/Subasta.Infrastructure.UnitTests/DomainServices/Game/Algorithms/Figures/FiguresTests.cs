using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Subasta.Domain.Game;
using Subasta.Infrastructure.UnitTests.Tools.Autofixture;

namespace Subasta.Infrastructure.UnitTests.DomainServices.Game.Algorithms.Figures
{
	[TestFixture]
	internal abstract class FiguresTests<TFigure> where TFigure : class,IFigure
	{
		private TestContext _context;

		[SetUp]
		protected virtual void OnSetUp()
		{
			_context = GetTestContext();
		}

		protected abstract TestContext GetTestContext();

		[Test]
		public  void CanPerform_IsAvailable()
		{
			_context.Sut.IsAvailable(_context.Status,)
		}

		[Test]
		public  void CanPerform_IsAvailable_WhenExceedPoints()
		{
			_context.Sut.IsAvailable(_context.Status,)
		}

		[Test]
		public  void CanPerform_IsAvailable_WhenItDoesNotExist()
		{
			throw new NotImplementedException();
		}

		[Test]
		public  bool CanPerform_IsAvailable_WhenItWasMarked()
		{
			throw new NotImplementedException();
		} 

		[Test]
		public  void CanGetSay()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void CanGetAlternativeSay()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void CanGetPoints()
		{
			throw new NotImplementedException();
		}

		[Test]
		public void CanGetAlternativePoints()
		{
			throw new NotImplementedException();
		}

		internal abstract class TestContext
		{
			private readonly IFixture _fixture = new Fixture().Customize(new SubastaAutoFixtureCustomizations());
			private TFigure _sut;
			private ISaysStatus _saysStatus;
			public TestContext()
			{
				_saysStatus = _fixture.Freeze<ISaysStatus>();
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
		}
}
}
