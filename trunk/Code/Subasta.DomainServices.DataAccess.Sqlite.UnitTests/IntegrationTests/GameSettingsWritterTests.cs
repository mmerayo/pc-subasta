using System;
using NUnit.Framework;
using Subasta.ApplicationServices;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess.Sqlite.Infrastructure;
using Subasta.DomainServices.DataAccess.Sqlite.Writters;
using Subasta.Infrastructure.ApplicationServices;
using Subasta.Infrastructure.Domain;

namespace Subasta.DomainServices.DataAccess.Sqlite.UnitTests.IntegrationTests
{
    [TestFixture]
    public class GameSettingsWritterTests
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
        public void CanStoreGame()
        {

            _context.Sut.StoreGameInfo(_context.GameId,1,1,Suit.FromName("Oros"),new []{new Card("o1")},new []{new Card("o2")},new []{new Card("o3")},new []{new Card("o4")} );

            //TODO: ASSERTions
            // get reader and read the data
        }

        private class TestContext : IDisposable
        {
            private readonly DbEngine _db;
            private GameSettingsWritter _sut;
            public readonly Guid GameId = Guid.NewGuid();

            public TestContext()
            {
                PathHelper = new PathUtils();
                _db = new DbEngine(PathHelper, false, "Dbs");
                _db.CreateDatabase(GameId);
            }

            public GameSettingsWritter Sut
            {
                get { return _sut ?? (_sut = new GameSettingsWritter(_db)); }
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