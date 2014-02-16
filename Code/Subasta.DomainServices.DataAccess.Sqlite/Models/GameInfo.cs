using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class GameInfo
    {

        public virtual int Id { get; set; }
        public virtual int FirstPlayer { get; set; }

        public virtual string TrumpSuit { get; set; }

        public virtual IEnumerable <GameInfo> CardsP1 { get; set; }

        public virtual IEnumerable<GameInfo> CardsP2 { get; set; }
        public virtual IEnumerable<GameInfo> CardsP3 { get; set; }
        public virtual IEnumerable<GameInfo> CardsP4 { get; set; }
    }
}
