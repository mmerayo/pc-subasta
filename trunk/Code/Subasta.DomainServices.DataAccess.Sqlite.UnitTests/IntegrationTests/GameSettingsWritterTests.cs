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
            new Bootstrapper().Execute();
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
            var gameId = Guid.NewGuid();

            _context.Sut.StoreGameInfo(gameId,1,Suit.FromName("Oros"),new []{new Card("o1")},new []{new Card("o2")},new []{new Card("o3")},new []{new Card("o4")} );

            //TODO: ASSERTions
            // get reader and read the data
        }

        private class TestContext : IDisposable
        {
            private readonly DbEngine _db;
            private GameSettingsWritter _sut;
            public TestContext()
            {
                PathHelper = new PathUtils();
                _db = new DbEngine(PathHelper, false, PathHelper.GetApplicationFolderPath("Dbs"));
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
              _db.Dispose();
            }

            ~TestContext()
            {
                Dispose(false);
            }

        }
    }
}