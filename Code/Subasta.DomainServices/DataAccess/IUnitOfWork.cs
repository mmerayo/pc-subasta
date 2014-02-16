using System;

namespace Subasta.DomainServices.DataAccess
{
    public interface IUnitOfWork<TSession>:IDisposable
    {
        TSession Session { get; }
    }

    

}