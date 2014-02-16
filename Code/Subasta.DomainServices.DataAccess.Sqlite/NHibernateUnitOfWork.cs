using NHibernate;

namespace Subasta.DomainServices.DataAccess.Sqlite
{
    internal class NHibernateUnitOfWork:IUnitOfWork<ISession>
    {
        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public ISession Session
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}