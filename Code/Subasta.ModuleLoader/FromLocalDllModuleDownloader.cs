using System.IO;

namespace Subasta
{
	internal class FromLocalDllModuleDownloader : ModuleDownloader
	{
		private const string Source = @"C:\5e9efe18-39a4-42a5-a0b9-bbd0b4bb6df0\pcsubasta\Publish";

		public override bool Update()
		{
			try
			{
				File.Copy(Path.Combine(Source, "Subasta.Lib.dll"), LibInvoker.TargetLibFile, true);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}