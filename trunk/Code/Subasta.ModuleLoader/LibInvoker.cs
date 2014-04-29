using System;
using System.IO;
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
			Assembly.LoadFrom(TargetLibFile);

			Type.GetType("Subasta.Lib.LibStarter").GetMethod("Start", BindingFlags.Static | BindingFlags.Public).Invoke(null,null);

		}

		
	}
}