using System;
using System.Data;
using NHibernate;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
    internal class NHibernateUnitOfWork : IUnitOfWork<ISession>
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;


        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                return;

            try
            {
                _session.Flush();
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

        public ISession Session
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

        ~NHibernateUnitOfWork()
        {
            Dispose(false);
        }
    }
}