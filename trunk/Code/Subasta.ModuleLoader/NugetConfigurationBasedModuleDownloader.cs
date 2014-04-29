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

			string repoUrl = ConfigurationManager.AppSettings["nuGetRepositoryUrl"];
			var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
			var latest = repo.FindPackagesById(PackageId).OrderByDescending(x => x.Version).First();
			if (!LibInvoker.TargetExists || latest.Version.Version > LibInvoker.TargetLibVersion)
			{
				string currentPath = Utils.GetCurrentPath();
				DeletePreviousInstallations(currentPath);
				InstallPackage(repo, currentPath, latest);

				ReplaceLibrary(currentPath, latest);
				DeleteFolder(GetDeploymentFolder(currentPath, latest.Version.Version.ToString()));
			}



			return true;
		}

		private static void DeletePreviousInstallations(string currentPath)
		{
			foreach (var directory in Directory.GetDirectories(currentPath,PackageId+"*"))
			{
				DeleteFolder(directory);
			}
			
		}

		private static void DeleteFolder(string deploymentFolder)
		{
			if (Directory.Exists(deploymentFolder))
				Directory.Delete(deploymentFolder, true);
		}

		private static void ReplaceLibrary(string currentPath, IPackage latest)
		{
			Version version = latest.Version.Version;
			string versionString = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor.ToString("0000"), version.Build.ToString("0000"), version.Revision.ToString("0000"));
			string deploymentFolder = GetDeploymentFolder(currentPath, versionString);

			File.Copy(Path.Combine(deploymentFolder, LibInvoker.TargetLibFileName), Path.Combine(currentPath, LibInvoker.TargetLibFileName), true);
			Directory.Delete(deploymentFolder, true);
		}

		private static string GetDeploymentFolder(string currentPath, string versionString)
		{
			return Path.Combine(currentPath, string.Format("{0}.{1}", PackageId, versionString));
		}

		private static void InstallPackage(IPackageRepository repo, string currentPath, IPackage latest)
		{
			

			var packageManager = new PackageManager(repo, currentPath);
			packageManager.InstallPackage(PackageId, latest.Version);
		}
	}
}