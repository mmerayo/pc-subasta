using System;
using NUnit.Framework;
using Subasta.ApplicationServices;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.DomainServices.DataAccess.Sqlite.Infrastructure;
using Subasta.DomainServices.DataAccess.Sqlite.Writters;
using Subasta.Infrastructure.ApplicationServices;
using Subasta.Infrastructure.Domain;
using Subasta.Infrastructure.DomainServices.Game;

namespace Subasta.DomainServices.DataAccess.Sqlite.UnitTests.IntegrationTests
{
    [TestFixture]
    public class ResultsWritterTests
    {

        [TestFixtureSetUp]
        public void OnFixtureSetUp()
        {
            new Bootstrap().Execute();
        }

        private TestContext _context;

        [SetUp]
        public void OnSetUp()
        {
            _context = new TestContext();
        }


        [Test]
        public void CanStoreNodeResult()
        {
            ISuit trump = Suit.FromName("Oros");
            var cardsP1 = new []{new Card("o1")};
            var cardsP2 = new []{new Card("o2")};
            var cardsP3 = new []{new Card("o3")};
            var cardsP4 = new []{new Card("o4")};
            _context.GameSettingsWritter.StoreGameInfo(_context.GameId,1,trump,cardsP1,cardsP2,cardsP3,cardsP4 );
            var status = new Status(_context.GameId,new CardComparer(trump),trump,new PlayerDeclarationsChecker() );
            status.SetPlayerBet(1);
            status.SetCards(1,cardsP1);
            status.SetCards(2, cardsP2);
            status.SetCards(3, cardsP3);
            status.SetCards(4, cardsP4);
            status.CurrentHand.Add(1, new Card("b1"));
            status.CurrentHand.Add(2, new Card("b2"));
            status.CurrentHand.Add(3, new Card("b3"));
            status.CurrentHand.Add(4, new Card("b4"));
            
            _context.Sut.Add(new NodeResult(status));

            //TODO: ASSERTions
            // get reader and read the data
        }

        private class TestContext : IDisposable
        {
            private readonly DbEngine _db;
            private GameSettingsWritter _gameSettingsWritter;
            private ResultStoreWritter _sut;
            public readonly Guid GameId = Guid.NewGuid();

            public TestContext()
            {
                PathHelper = new PathUtils();
                _db = new DbEngine(PathHelper, false, PathHelper.GetApplicationFolderPath("Dbs"));
                _db.CreateDatabase(GameId);
            }

            public GameSettingsWritter GameSettingsWritter
            {
                get { return _gameSettingsWritter ?? (_gameSettingsWritter = new GameSettingsWritter(_db)); }
            }

            public ResultStoreWritter Sut
            {
                get { return _sut ?? (_sut = new ResultStoreWritter(_db)); }
            }

            private IPathHelper PathHelper { get; set; }
            

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool disposing)
            {
             // _db.DropDatabase();
            }

            ~TestContext()
            {
                Dispose(false);
            }

        }
    }
}