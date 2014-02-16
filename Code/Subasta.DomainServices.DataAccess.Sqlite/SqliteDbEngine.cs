using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Subasta.ApplicationServices;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
	internal class SqliteDbEngine : IDbHelper, IDisposable
	{
		private readonly IPathHelper _pathHelper;
		private readonly string _dbFolderPath;
		protected bool InMemory { get; private set; }
		private readonly object _syncLock = new object();
		private string DbName { get; set; }
		private SQLiteConnection _inMemoryConnection = null; //holds the connection in memory


		public SqliteDbEngine(IPathHelper pathHelper, bool inMemory,string dbFolderPath)
		{
			_pathHelper = pathHelper;
			_dbFolderPath = dbFolderPath;

			InMemory = inMemory;
		}

		public bool Created { get; private set; }

		private string FilePath
		{
			get
			{
				if (!InMemory)
					if (string.IsNullOrEmpty(_dbFolderPath))
						return _pathHelper.GetApplicationFolderPathForFile(DbName);
					else
						return Path.Combine(_dbFolderPath, DbName);
				throw new InvalidOperationException("In memory dont have a file");
			}
		}

		public void CreateDatabase(Guid gameId)
		{
			//its created automatically
			//DropDatabase(); //to keep the same behavior as sqlserver //TODO: THIS MIGHT NEED TO BE REMOVED
			Debug.Assert(gameId!=Guid.Empty);
			
			DbName = string.Format("db{0}.game", gameId.ToString().Replace('-', '_'));
			

			if (InMemory)
			{
				lock (_syncLock)
				{
					Debug.Assert(_inMemoryConnection == null);
					_inMemoryConnection = new SQLiteConnection(GetConnectionString());
					_inMemoryConnection.Open();

					CreateSchema();
				}
			}
			Created = true;
		}

		private void CreateSchema()
		{
			throw new NotImplementedException();
		}

		public void DropDatabase()
		{
			lock (_syncLock)
			{
				if (!InMemory)
				{
					DeleteDbFile(3);
					Created = false;
					//DataBase = null;

				}
				else
				{
					if (_inMemoryConnection != null)
					{
						_inMemoryConnection.Dispose();
						_inMemoryConnection = null;
					}
				}
			}
		}

		private void DeleteDbFile(int tries)
		{
			if (tries == 0)
				return;
			try
			{
				if (File.Exists(FilePath))
					File.Delete(FilePath);
			}
			catch (Exception)
			{
				Thread.Sleep(150);
				DeleteDbFile(--tries);
			}
		}

		public string GetConnectionString()
		{
			return !InMemory
							   ? string.Format("Data Source={0};Version=3;BinaryGuid=False;", FilePath)
							   : string.Format("FullUri=file:{0}?mode=memory&cache=shared;Version=3;BinaryGuid=False;", DbName);
		}

		private bool _disposed = false;

		public void Dispose()
		{
			if (!_disposed)
				lock (_syncLock)
					if (!_disposed)
					{
						DropDatabase();
						_disposed = true;
					}

		}
	}
}
