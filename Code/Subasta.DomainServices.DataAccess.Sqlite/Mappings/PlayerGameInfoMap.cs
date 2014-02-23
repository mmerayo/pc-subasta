using FluentNHibernate.Mapping;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Mappings
{
	internal class PlayerGameInfoMap : ClassMap<PlayerGameInfo>
	{
		public PlayerGameInfoMap()
		{
			Table("PlayerGameInfo");
			Id(x => x.Id).GeneratedBy.Identity();
			Map(x => x.PlayerPosition).Not.Nullable();
			
			HasManyToMany(x => x.Cards);
		}
	}
}