using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Subasta
{
	internal class FromLocalDllModuleDownloader : ModuleDownloader
	{
		private const string Source = @"C:\5e9efe18-39a4-42a5-a0b9-bbd0b4bb6df0\pcsubasta\Publish";
		public override bool Update()
		{

			try
			{
				string sourceFileName = Path.Combine(Source, "Subasta.Lib.dll");
				if(new FileInfo(sourceFileName).Directory!=new DirectoryInfo(LibInvoker.TargetLibFile))
					File.Copy(sourceFileName, LibInvoker.TargetLibFile, true);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}