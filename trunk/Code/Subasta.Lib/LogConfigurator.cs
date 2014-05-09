using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Subasta.Infrastructure.ApplicationServices.IO;

namespace Subasta.Lib
{
	public static class LogConfigurator
	{
		public static void ConfigureLogging()
		{
			string applicationFolderPathForFile = new PathUtils().GetApplicationFolderPathForFile("Log", "error.log", true);
			var fileAppender = GetFileAppender(applicationFolderPathForFile, Level.Error);
			BasicConfigurator.Configure((IAppender) fileAppender);
			((Hierarchy) LogManager.GetRepository()).Root.Level = Level.Debug;
		}

		private static IAppender GetFileAppender(string logFile,Level thresholdLevel)
		{
			var layout = new PatternLayout("%utcdate %-5level %thread - %logger - %message - %exception - %newline ");
			layout.ActivateOptions(); // According to the docs this must be called as soon as any properties have been changed.

			var appender = new FileAppender
			{
				File = logFile,
				Encoding = Encoding.UTF8,
				Threshold = thresholdLevel,
				Layout = layout
			};

			appender.ActivateOptions();

			return appender;
		}
	}
}