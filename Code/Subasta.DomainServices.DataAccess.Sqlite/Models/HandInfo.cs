using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class HandInfo
    {
        public virtual int Id { get; set; }
        public virtual string Declaration { get; set; }
        public virtual int FirstPlayer { get; set; }
        public virtual string CardP1 { get; set; }
        public virtual string CardP2 { get; set; }
        public virtual string CardP3 { get; set; }
        public virtual string CardP4 { get; set; }

        public virtual int Sequence{get; set; }
        public virtual int ExplorationId { get; set; }

        //public virtual ExplorationInfo Exploration { get; set; }
    }
}