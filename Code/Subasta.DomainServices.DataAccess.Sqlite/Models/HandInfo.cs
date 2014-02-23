using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class HandInfo
    {
        public virtual int Id { get; set; }
        public virtual string Declaration { get; set; }
        public virtual int FirstPlayer { get; set; }
        public virtual IList<CardInfo> Cards { get; set; }

        public virtual int Sequence{get; set; }
        public virtual int ExplorationId { get; set; } 
    }
}