using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Subasta
{
	partial class FrmActualizando
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblMessage = new Label();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new Point(27, 13);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new Size(159, 13);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Comprobando Actualizaciones...";
			this.lblMessage.UseWaitCursor = true;
			// 
			// FrmActualizando
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.ClientSize = new Size(213, 50);
			this.ControlBox = false;
			this.Controls.Add(this.lblMessage);
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			this.Name = "FrmActualizando";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Subasta";
			this.UseWaitCursor = true;
			this.Shown += new EventHandler(this.FrmActualizando_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label lblMessage;
	}
}