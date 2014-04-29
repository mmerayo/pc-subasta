using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NuGet;

namespace Subasta
{
	internal class NugetConfigurationBasedModuleDownloader : ModuleDownloader
	{
		private const string PackageId = "Subasta.Lib";
		public override bool Update()
		{
			string repoUrl = ConfigurationManager.AppSettings["nuGetRepositoryUrl"];
			var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
			var packages = repo.FindPackagesById(PackageId).ToList();
			return false;
		}
	}
}