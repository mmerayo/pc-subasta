using System.IO;
using System.Linq;
using Subasta.Domain.Deck;
using Subasta.Infrastructure.Domain;

namespace Subasta.Client.Common.Games
{
	internal class StoredGameReader : IStoredGameReader
	{
		public StoredGameData Load(string fileName)
		{
			var result = new StoredGameData();

			Stream stream = null;
			if ((stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) != null)
			{

				using (stream)
				using (var sr = new StreamReader(stream))
				{
					string line;
					var index = 0;
					var cards = new ICard[4][];

					while ((line = sr.ReadLine()) != null)
					{
						string[] strings = line.Split(' ');
						cards[index++] = strings.Select(x => new Card(x)).ToArray();
					}
					result.Player1Cards = cards[0];
					result.Player2Cards = cards[1];
					result.Player3Cards = cards[2];
					result.Player4Cards = cards[3];

				}

			}
			return result;
		}
	}
}