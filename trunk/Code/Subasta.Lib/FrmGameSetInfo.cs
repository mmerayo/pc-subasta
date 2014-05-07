using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain;
using Subasta.Domain.Deck;
using Subasta.Domain.Game;
using Subasta.Infrastructure.Domain;
using Subasta.Lib.Interaction;
using Subasta.Lib.UserControls;

namespace Subasta.Lib
{
	public partial class FrmGameSetInfo : Form
	{
		public FrmGameSetInfo()
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