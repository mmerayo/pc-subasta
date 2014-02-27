using System;
using System.IO;
using NUnit.Framework;
using Subasta.ApplicationServices;
using Subasta.Infrastructure.ApplicationServices;

namespace Subasta.DomainServices.DataAccess.Sqlite.UnitTests.IntegrationTests
{
    [TestFixture]
    public class DbEngineTests
    {
        private TestContext _context;

        [SetUp]
        public void OnSetUp()
        {
            _context = new TestContext();
        }

        [Test]
        public void CanCreateDatabase()
        {
            var gameId = Guid.NewGuid();

            _context.Sut.CreateDatabase(gameId);

            //TODO: ASSERTions
        }


        private class TestContext
        {
            public TestContext()
            {
                PathHelper = new PathUtils();
                Sut = new DbEngine(PathHelper, false, "Dbs");
            }

            public DbEngine Sut { get; private set; }

            private IPathHelper PathHelper { get; set; }
        }

    }
}
