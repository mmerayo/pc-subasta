using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common;
using Subasta.Infrastructure.IoC;

namespace Analyzer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			IoCRegistrator.Register(new List<Registry>
				{
					new RegisterClientCommonIoc(),
					new RegisterAnalyzerIoc()
				});
			Application.Run(ObjectFactory.GetInstance<FrmMain>());
		}
	}
}
