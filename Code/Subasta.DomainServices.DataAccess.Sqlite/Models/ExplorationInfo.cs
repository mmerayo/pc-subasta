using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
    internal class ExplorationInfo
    {
        public virtual List<HandInfo> Hands { get; set; }
        public virtual int PointsTeam1 { get; set; }
        public virtual int PointsTeam2 { get; set; }
    }
}