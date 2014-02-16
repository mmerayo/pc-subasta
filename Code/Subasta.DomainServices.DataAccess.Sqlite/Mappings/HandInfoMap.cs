using FluentNHibernate.Mapping;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Mappings
{
    internal class HandInfoMap : ClassMap<HandInfo>
    {
        public HandInfoMap()
        {
            Table("Hands");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Declaration).Length(20).Nullable();
            Map(x => x.FirstPlayer).Not.Nullable();
            Map(x => x.Sequence).Not.Nullable();
            Map(x => x.ExplorationId).Not.Nullable();
            HasManyToMany(x => x.Cards);
        }

    }
}