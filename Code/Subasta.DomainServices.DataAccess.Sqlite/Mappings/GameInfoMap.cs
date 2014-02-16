using FluentNHibernate.Mapping;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Mappings
{
    internal class GameInfoMap : ClassMap<GameInfo>
    {
        public GameInfoMap()
        {
            Table("GameInfo");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.TrumpSuit).Length(7).Not.Nullable();
            Map(x => x.FirstPlayer).Not.Nullable();
            HasManyToMany(x => x.CardsP1);
            HasManyToMany(x => x.CardsP2);
            HasManyToMany(x => x.CardsP3);
            HasManyToMany(x => x.CardsP4);
        }
    }
}