using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Subasta.DomainServices.DataAccess;
using Subasta.DomainServices.Game;
using Subasta.Infrastructure.ApplicationServices;

namespace Subasta.Infrastructure.UnitTests.DomainServices
{
	[TestFixture]
	class GameGeneratorTests
	{
		private TestContext _context;

		[SetUp]
		public void OnSetUp()
		{
			_context=new TestContext();
		}

		[Test]
		public void Can_GenerateNewGame()
		{
			_context.WithGenerateNewGameExpectations();

			Guid result=_context.Sut.GenerateNewGame();

			Assert.That(result, Is.Not.Empty);
			_context.VerifySuffleWasCalled();
			_context.VerifyGameExplorerWasCalled();
			_context.VerifyGameWasCreated();
			_context.VerifyGameWasCreationWasFinished();
		}

		[Test]
		public void When_GenerateNewGame_Fails_MarksTheGameGenerationResultAsFailed()
		{
			_context.WithGenerateNewGameExpectations().ExpectFailureWhileGenerating();

			Guid result = _context.Sut.GenerateNewGame();

			Assert.That(result, Is.Not.Empty);
			_context.VerifySuffleWasCalled();
			_context.VerifyGameExplorerWasCalled();
			_context.VerifyGameWasCreated();
			_context.VerifyGameWasCreationWasFinished();
			_context.VerifyGameWasMarkedAsFail();
		}

		private class TestContext
		{
			private readonly IFixture _fixture;
			private Mock<IGameExplorer> _gameExplorer;
			private Mock<IDeckSuffler> _suffler;
			private Mock<IGameDataAllocator> _dataAllocator;
			public TestContext()
			{
				_fixture=new Fixture().Customize(new AutoMoqCustomization());
				_gameExplorer = _fixture.Freeze<Mock<IGameExplorer>>();
				_suffler = _fixture.Freeze < Mock<IDeckSuffler>>();
				_dataAllocator = _fixture.Freeze < Mock<IGameDataAllocator>>();
			}

			public GameGenerator Sut
			{
				get { return _fixture.CreateAnonymous<GameGenerator>(); }
			}

			public void VerifySuffleWasCalled()
			{
				throw new NotImplementedException();
			}

			public void VerifyGameExplorerWasCalled()
			{
				throw new NotImplementedException();
			}

			public void VerifyGameWasCreated()
			{
				throw new NotImplementedException();
			}

			public TestContext WithGenerateNewGameExpectations()
			{
				//_gameExplorer.Expect(x=>x.Execute())
				throw new NotImplementedException();

				return this;
			}

			public void VerifyGameWasCreationWasFinished()
			{
				throw new NotImplementedException();
			}

			public TestContext ExpectFailureWhileGenerating()
			{
				throw new NotImplementedException();
			}

			public void VerifyGameWasMarkedAsFail()
			{
				throw new NotImplementedException();
			}
		}
	}
}
