using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NuGet;

namespace Subasta
{
	internal class NugetConfigurationBasedModuleDownloader : ModuleDownloader
	{
		private const string PackageId = "Subasta.Lib";
		public override bool Update()
		{
			try
			{
				string repoUrl = ConfigurationManager.AppSettings["nuGetRepositoryUrl"];
				var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
				var latest = repo.FindPackagesById(PackageId).OrderByDescending(x => x.Version).First();
				if (!LibInvoker.TargetExists|| latest.Version.Version > LibInvoker.TargetLibVersion)
				{
					string currentPath = Utils.GetCurrentPath();
					InstallPackage(repo, currentPath, latest);

					ReplaceLibrary(currentPath, latest);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		private static void ReplaceLibrary(string currentPath, IPackage latest)
		{
			string deploymentFolder = GetDeploymentFolder(currentPath, latest);

			File.Copy(Path.Combine(deploymentFolder, LibInvoker.TargetLibFileName), currentPath, true);
			Directory.Delete(deploymentFolder, true);
		}

		private static string GetDeploymentFolder(string currentPath, IPackage latest)
		{
			return Path.Combine(currentPath, string.Format("{0}.{1}", PackageId, latest.Version.Version));
		}

		private static void InstallPackage(IPackageRepository repo, string currentPath, IPackage latest)
		{
			string deploymentFolder = GetDeploymentFolder(currentPath, latest);
			if (Directory.Exists(deploymentFolder))
				Directory.Delete(deploymentFolder, true);

			var packageManager = new PackageManager(repo, currentPath);
			packageManager.InstallPackage(PackageId, latest.Version);
		}
	}
}