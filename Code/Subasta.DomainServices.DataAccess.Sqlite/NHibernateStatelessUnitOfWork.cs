using System;
using NHibernate;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
	internal class NHibernateStatelessUnitOfWork : IUnitOfWork<IStatelessSession>
	{
		private readonly IStatelessSession _session;
		private readonly ITransaction _transaction;


		public NHibernateStatelessUnitOfWork(ISessionFactory sessionFactory)
		{
			_session = sessionFactory.OpenStatelessSession();
			_transaction = _session.BeginTransaction();
		}

		public void Commit()
		{
			if (_transaction == null)
				return;

			try
			{
				_transaction.Commit();
			}
			catch (Exception ex)
			{
				//TODO: LOG
				_transaction.Rollback();
				throw;
			}
			finally
			{
				_transaction.Dispose();
			}
		}

		public IStatelessSession Session
		{
			get { return _session; }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed;

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{

				}
				if (_transaction != null)
					_transaction.Dispose();
				_disposed = true;
			}
		}

		~NHibernateStatelessUnitOfWork()
		{
			Dispose(false);
		}
	}
}