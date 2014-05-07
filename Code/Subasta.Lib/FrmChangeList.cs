using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Subasta.Client.Common.Images;

namespace Subasta.Lib
	{
		public partial class FrmChangeList : Form
		{

			public FrmChangeList(IResourceReadingUtils resourceReadingUtils)
			{
				InitializeComponent();

				this.txtChanges.Text = resourceReadingUtils.GetText("ChangeList.txt");
			}
		}
	}
