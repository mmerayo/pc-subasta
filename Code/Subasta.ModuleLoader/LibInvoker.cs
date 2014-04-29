using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Subasta
{
	internal class LibInvoker
	{
		public static string TargetLibFile
		{
			get { return Path.Combine(Utils.GetCurrentPath(), TargetLibFileName); }
		}

		public static string TargetLibFileName { get { return "Subasta.Lib.dll"; }}

		public static Version TargetLibVersion
		{
			get
			{
				string productVersion = FileVersionInfo.GetVersionInfo(TargetLibFile).ProductVersion;
				return new Version(productVersion);
			}
		}

		public static bool TargetExists
		{
			get { return File.Exists(TargetLibFile); }
		}

		public static void Initialize()
		{
			Assembly assembly = Assembly.LoadFrom(TargetLibFile);
			Type type = assembly.GetTypes().Single(x => x.Name == "LibStarter");
			MethodInfo methodInfo = type.GetMethod("Start", BindingFlags.Static | BindingFlags.Public);
			methodInfo.Invoke(null,null);

		}

		
	}
}