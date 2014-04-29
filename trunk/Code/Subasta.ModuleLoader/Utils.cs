using System;
using System.IO;
using System.Reflection;

namespace Subasta
{
	public static class Utils
	{
		public static string GetAssemblyPath()
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return Path.GetDirectoryName(path);

		}
	}
}