using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class GameInfo
    {
        public virtual int FirstPlayer { get; set; }

        public virtual string TrumpSuit { get; set; }

        public virtual IEnumerable <CardInfo> CardsP1 { get; set; }

        public virtual IEnumerable<CardInfo> CardsP2 { get; set; }
        public virtual IEnumerable<CardInfo> CardsP3 { get; set; }
        public virtual IEnumerable<CardInfo> CardsP4 { get; set; }
    }
}
