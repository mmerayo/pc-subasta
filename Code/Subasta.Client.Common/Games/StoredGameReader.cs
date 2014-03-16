using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
					var deserialized=JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());

					result.Player1Cards = ((string)deserialized.Player1Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
					result.Player2Cards = ((string)deserialized.Player2Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
					result.Player3Cards = ((string)deserialized.Player3Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
					result.Player4Cards = ((string)deserialized.Player4Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
					result.ExplorationDepth = deserialized.ExplorationDepth;
				}

			}
			return result;
		}
	}
}