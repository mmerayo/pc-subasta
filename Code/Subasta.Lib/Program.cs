using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using StructureMap.Configuration.DSL;
using Subasta.Client.Common;
using Subasta.Client.Common.Infrastructure;
using Subasta.Infrastructure.IoC;

namespace Subasta
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			//TODO: move to module
			IoCRegistrator.Register(new List<Registry>
			                        {
			                        	new RegisterClientCommonIoc(),
			                        	new RegisterIoc()
			                        });
			Application.Run(ObjectFactory.GetInstance<FrmMain>());
		}
	}
}
