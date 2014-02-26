namespace Subasta.ApplicationServices
{
	public interface IPathHelper
	{
		string GetApplicationFolderPathForFile(string fileName);
		string GetApplicationFolderPathForFile(string dbFolderPath, string fileName, bool createIfNotExists=false);

		string GetApplicationFolderPath(string folderName,bool createIfNotExists=false);
		string GetApplicationFolderPath();
		string GetPath(string uri);
		void CopyFolder(string sourceFolder, string destFolder, bool deleteIfExists = true);
	}
}
