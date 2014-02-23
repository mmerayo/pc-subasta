using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Subasta.Domain.Deck;
using Subasta.DomainServices.DataAccess.Sqlite.Models;

namespace Subasta.DomainServices.DataAccess.Sqlite.Writters
{
    internal static class StaticDataReader
    {
        public static IEnumerable<CardInfo> GetDbCards(ISession session, IEnumerable<ICard> cards)
        {
            return cards.Select(card => GetDbCard(session, card.Suit.Name, card.Number)).ToList();
        }

        private static CardInfo GetDbCard(ISession session, string suit, int number)
        {
            return session.QueryOver<CardInfo>().Where(x => x.Suit == suit).And(y => y.Number == number)
                .SingleOrDefault();
        }

        public static IList<CardInfo> GetDbCards(ISession session, IEnumerable<CardInfo> cards)
        {
            return cards.Select(card => GetDbCard(session, card.Suit, card.Number)).ToList();
        }
    }
}