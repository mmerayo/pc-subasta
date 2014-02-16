using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class HandInfo
    {
        public virtual string Declaration { get; set; }
        public virtual int FirstPlayer { get; set; }
        public virtual List<CardInfo> Cards { get; set; }

    }
}