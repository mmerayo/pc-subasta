using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class CardInfo
    {
        public virtual int Id { get; set; }
        public virtual string Suit { get; set; }
        public virtual int Number { get; set; }
    }
}