using System;
using System.IO;
using System.Reflection;
using Subasta.ApplicationServices.IO;

namespace Subasta.Infrastructure.ApplicationServices.IO
{
	class PathUtils:IPathHelper
	{
		private string _applicationFolderPath;

		
		public string GetApplicationFolderPath(string folderName,bool createIfNotExists=false)
		{
		    string path = string.Format("{0}\\{1}", GetApplicationFolderPath(), folderName);
            EnsureDirectoryExists(path);

		    return path;
		}

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                lock (this)
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
        }

	    public string GetApplicationFolderPath()
		{
			if (_applicationFolderPath == null)
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				_applicationFolderPath = Path.GetDirectoryName(path);
			}
			return _applicationFolderPath;
		}

		public string GetPath(string uri)
		{
			var uriBuilder = new UriBuilder(uri);
			string path = Uri.UnescapeDataString(uriBuilder.Path);
			_applicationFolderPath = Path.GetDirectoryName(path);
			return _applicationFolderPath;
		}

		

		public string GetApplicationFolderPathForFile(string fileName)
		{
			return Path.Combine(GetApplicationFolderPath(), fileName);
		}


		public string GetApplicationFolderPathForFile(string folderName, string fileName, bool createIfNotExists = false)
		{

			return Path.Combine(GetApplicationFolderPath(folderName, createIfNotExists), fileName);
		}

		public void CopyFolder(string sourceFolder, string destFolder, bool deleteIfExists = true)
		{
			string[] files = Directory.GetFiles(sourceFolder);
			string[] folders = Directory.GetDirectories(sourceFolder);

			if (deleteIfExists && Directory.Exists(destFolder))
				Directory.Delete(destFolder, true);

			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);

			string name;
			string dest;
			foreach (string file in files)
			{
				name = Path.GetFileName(file);
				dest = Path.Combine(destFolder, name);
				File.Copy(file, dest);
			}

			foreach (string folder in folders)
			{
				name = Path.GetFileName(folder);
				dest = Path.Combine(destFolder, name);
				CopyFolder(folder, dest);
			}
		}
	}
}
