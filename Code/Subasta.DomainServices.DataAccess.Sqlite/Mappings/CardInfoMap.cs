using FluentNHibernate.Mapping;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Mappings
{
    internal class CardInfoMap:ClassMap<CardInfo>
    {
        public CardInfoMap()
        {
            Table("Cards");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Suit).Length(7).Not.Nullable();
            Map(x => x.Number).Not.Nullable();
        }
    }
}