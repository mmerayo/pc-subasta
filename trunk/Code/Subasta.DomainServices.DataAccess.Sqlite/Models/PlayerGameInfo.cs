using System.Collections.Generic;
using Remotion.Linq.Collections;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class PlayerGameInfo
    {
        public PlayerGameInfo(){}
        public PlayerGameInfo(int position, IEnumerable<CardInfo> cards)
        {
            PlayerPosition = position;
            Cards = new List<CardInfo>( cards);
        }

        public virtual int Id { get; set; }

        public virtual GameInfo Game { get; set; }
        public virtual int PlayerPosition { get; set; }
        public virtual IList<CardInfo> Cards { get; set; }
    }
}