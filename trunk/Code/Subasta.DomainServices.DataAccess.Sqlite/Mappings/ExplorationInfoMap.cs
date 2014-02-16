using FluentNHibernate.Mapping;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Mappings
{
    internal class ExplorationInfoMap : ClassMap<ExplorationInfo>
    {
        public ExplorationInfoMap()
        {
            Table("ExplorationInfo");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.PointsTeam1).Not.Nullable();
            Map(x => x.PointsTeam1).Not.Nullable();
            Map(x => x.GameId).Not.Nullable();
            HasMany(x => x.Hands);
        }
    }
}