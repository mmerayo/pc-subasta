using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subasta.ApplicationServices
{
	public interface IPathHelper
	{
		string GetApplicationFolderPathForFile(string fileName);
		string GetApplicationFolderPath(string folderName);
		string GetApplicationFolderPath();
		string GetPath(string uri);
		void CopyFolder(string sourceFolder, string destFolder, bool deleteIfExists = true);
	}
}
