using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class CardInfo
    {
        public virtual int Id { get; set; }
        public virtual string Suit { get; set; }
        public virtual int Number { get; set; }
        public virtual IList<HandInfo> Hands { get; set; }//TODO: consider REMOVE is here for the mapping
        public virtual IList<PlayerGameInfo> PlayerGameInfos { get; set; }//TODO: consider REMOVE is here for the mapping
    }
}