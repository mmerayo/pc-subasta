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

        public virtual IList <PlayerGameInfo>Players { get; set; }
       
    }
}
