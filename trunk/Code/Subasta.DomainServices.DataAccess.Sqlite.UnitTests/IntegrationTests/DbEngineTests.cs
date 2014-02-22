using System;
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
            }

            public DbEngine Sut { get { return new DbEngine(PathHelper, false, PathHelper.GetApplicationFolderPath("Dbs")); } }

            private IPathHelper PathHelper { get; set; }
        }

    }
}
