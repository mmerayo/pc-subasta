using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Lib.Interaction;
using Subasta.Lib.UserControls;

namespace Subasta.Lib
{
	public partial class FrmGameInfo : Form
	{
		public FrmGameInfo()
		{
			InitializeComponent();

			InitializeCustomUserControls();

		}

		private void InitializeCustomUserControls()
		{
			var userControls = this.FindControls<ICustomUserControl>();
			foreach (var userControl in userControls)
			{
				userControl.Initialize();
			}
		}

	}
}