using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Subasta.ApplicationServices.IO;
using Subasta.Domain.DalModels;
using Subasta.DomainServices.Dal;

namespace Subasta.Client.Common.Storage
{
	internal class StoredGameWritter:IStoredGameWritter
	{
		private readonly IPathHelper _pathHelper;

		public StoredGameWritter(IPathHelper pathHelper)
		{
			_pathHelper = pathHelper;
		}

		public void Write(StoredGameData source)
		{
			var data = GetJsonData(source);

			string fileName = Guid.NewGuid().ToString().Replace("-", "_")+".data";
			string gamesPath = _pathHelper.GetApplicationFolderPath("Games", true);
			Directory.Delete(gamesPath,true);
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
				GameId = source.GameId,
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