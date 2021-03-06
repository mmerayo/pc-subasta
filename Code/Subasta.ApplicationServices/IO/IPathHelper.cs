﻿namespace Subasta.ApplicationServices.IO
{
	public interface IPathHelper
	{
		string GetApplicationFolderPathForFile(string fileName);
		string GetApplicationFolderPathForFile(string folderName, string fileName, bool createIfNotExists=false);

		string GetApplicationFolderPath(string folderName,bool createIfNotExists=false);
		string GetApplicationFolderPath();
		string GetPath(string uri);
		void CopyFolder(string sourceFolder, string destFolder, bool deleteIfExists = true);
	}
}
