namespace Subasta.Client.Common.Games
{
	interface IStoredGameReader
	{
		StoredGameData Load(string file);
	}
}