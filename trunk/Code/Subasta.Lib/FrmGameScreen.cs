using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common.Extensions;
using Subasta.Lib.UserControls;

namespace Subasta.Lib
{
	public partial class FrmGameScreen : Form
	{
		public FrmGameScreen()
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
