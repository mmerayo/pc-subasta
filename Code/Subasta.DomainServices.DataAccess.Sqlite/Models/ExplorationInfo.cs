using System.Collections.Generic;

namespace Subasta.DomainServices.DataAccess.Sqlite.Models
{
	internal class ExplorationInfo
	{
		public virtual int Id { get; set; }
		public virtual int PointsTeam1 { get; set; }
		public virtual int PointsTeam2 { get; set; }

		public virtual GameInfo Game { get; set; }
		public virtual IList<HandInfo> Hands { get; set; }

		public virtual string Trump { get; set; }

		public virtual int TeamBet { get; set; }
	}
}