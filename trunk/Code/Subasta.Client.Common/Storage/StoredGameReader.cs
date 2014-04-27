using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Subasta.ApplicationServices.IO;
using Subasta.Domain.DalModels;
using Subasta.Domain.Deck;
using Subasta.DomainServices.Dal;
using Subasta.Infrastructure.Domain;

namespace Subasta.Client.Common.Storage
{
	internal class StoredGameReader : IStoredGameReader
	{
		private readonly IDeck _deck;

		public StoredGameReader(IDeck deck)
		{
			_deck = deck;
		}

		public StoredGameData LoadFromFile(string fileName)
		{

			StoredGameData result;
			Stream stream = null;
			if ((stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) != null)
			{
				string fileData;
				using (stream)
				using (var sr = new StreamReader(stream))
					fileData = sr.ReadToEnd();
				result = Load(fileData);

				return result;
			}
			throw new FileNotFoundException("File not found",fileName);
		}

		public StoredGameData Load(string data)
		{
			var result = new StoredGameData();
			var deserialized = JsonConvert.DeserializeObject<dynamic>(data);

			result.Player1Cards = ((string) deserialized.Player1Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
			result.Player2Cards = ((string) deserialized.Player2Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
			result.Player3Cards = ((string) deserialized.Player3Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();
			result.Player4Cards = ((string) deserialized.Player4Cards).Split(' ').ToArray().Select(x => new Card(x)).ToArray();

			result.Player1Type = (PlayerType) Enum.Parse(typeof (PlayerType), (string) deserialized.Player1Type);
			result.Player2Type = (PlayerType) Enum.Parse(typeof (PlayerType), (string) deserialized.Player2Type);
			result.Player3Type = (PlayerType) Enum.Parse(typeof (PlayerType), (string) deserialized.Player3Type);
			result.Player4Type = (PlayerType) Enum.Parse(typeof (PlayerType), (string) deserialized.Player4Type);

			result.FirstPlayer = deserialized.FirstPlayer;
			
			ThrowIfNotValidData(result.Player1Cards, result.Player2Cards, result.Player3Cards, result.Player4Cards);

			return result;
		}

		private void ThrowIfNotValidData(ICard[] player1Cards, ICard[] player2Cards, ICard[] player3Cards, ICard[] player4Cards)
		{
			var cards = new List<ICard>(_deck.Cards.Cards);

			if (player1Cards.Length != 10)
				throw new FileLoadException("player 1 cards not valid");
			if (player2Cards.Length != 10)
				throw new FileLoadException("player 2 cards not valid");
			if (player3Cards.Length != 10)
				throw new FileLoadException("player 3 cards not valid");
			if (player4Cards.Length != 10)
				throw new FileLoadException("player 4 cards not valid");

			var source = player1Cards.Concat(player2Cards).Concat(player3Cards).Concat(player4Cards);

			foreach (var card in source)
			{
				int removed = cards.RemoveAll(x => x.Equals(card));
				if (removed != 1)
					throw new FileLoadException(string.Format("Card {0} is duplicated", card.ToShortString()));
			}
			if (cards.Count > 0)
			{
				string nonDefined = string.Format("{0}",string.Join(",", cards.Select(x=>x.ToShortString())));
				throw new FileLoadException("The file data is not valid. Missing: " +nonDefined);
			}

		}
	}

	internal class StoredGameWritter:IStoredGameWritter
	{
		private IPathHelper _pathHelper;

		public StoredGameWritter(IPathHelper pathHelper)
		{
			_pathHelper = pathHelper;
		}

		public void Write(StoredGameData source)
		{
			var data = GetJsonData(source);

			string fileName = Guid.NewGuid().ToString().Replace("-", "_")+".data";
			string filePath = _pathHelper.GetApplicationFolderPathForFile("Games", fileName, true);

			Stream stream = null;
			if ((stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write)) != null)
			{
				using (stream)
				using (var sw = new StreamWriter(stream))
					sw.Write(data);

			}
		}

		private static dynamic GetJsonData(StoredGameData source)
		{
			dynamic target = new
			                 {
			                 	FirstPlayer = source.FirstPlayer,
			                 	Player1Cards = string.Join(" ", source.Player1Cards.Select(x => x.ToShortString())),
			                 	Player1Type = source.Player1Type.ToString(),
			                 	Player2Cards = string.Join(" ", source.Player2Cards.Select(x => x.ToShortString())),
			                 	Player2Type = source.Player2Type.ToString(),
			                 	Player3Cards = string.Join(" ", source.Player3Cards.Select(x => x.ToShortString())),
			                 	Player3Type = source.Player3Type.ToString(),
			                 	Player4Cards = string.Join(" ", source.Player4Cards.Select(x => x.ToShortString())),
			                 	Player4Type = source.Player4Type.ToString(),
			                 };
			var data = JsonConvert.SerializeObject(target);
			return data;
		}
	}
}