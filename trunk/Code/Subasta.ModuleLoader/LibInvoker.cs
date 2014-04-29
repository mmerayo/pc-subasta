using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Subasta
{
	internal class LibInvoker
	{
		public static string TargetLibFile
		{
			get { return Path.Combine(Utils.GetAssemblyPath(), "Subasta.Lib.dll"); }
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