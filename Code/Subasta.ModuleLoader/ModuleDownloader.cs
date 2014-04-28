﻿namespace Subasta
{
	internal abstract class ModuleDownloader
	{
		protected ModuleDownloader(){}
		private static  ModuleDownloader _instance=null;
		private static readonly object SyncLock=new object();
		public static ModuleDownloader Instance
		{
			get
			{
				if(_instance==null)
					lock(SyncLock)
						if (_instance == null)
						{
							_instance= new NugetConfigurationBasedModuleDownloader();
						}
				return _instance;
			}
		}

		/// <summary>
		/// returns false if the assembly is not ready to be loaded
		/// </summary>
		/// <returns></returns>
		public abstract bool GetLatest();
	}
}