using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subasta.Domain.Deck;

namespace Subasta.DomainServices.DataAccess
{
    public interface IGameSettingsStoreWritter
    {
        void StoreGameInfo(int firstPlayer, ISuit trump, ICard[] cardsP1, ICard[] cardsP2, ICard[] cardsP3, ICard[] cardsP4);

    }
}
