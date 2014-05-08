namespace Subasta.Lib
	{
	partial class FrmGameInfo
		{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.ucTricks1 = new Subasta.Lib.UserControls.UcTricks();
			this.ucSaySelector1 = new Subasta.Lib.UserControls.UcSaySelector();
			this.ucPlayerOverview1 = new Subasta.Lib.UserControls.UcPlayerOverview();
			this.SuspendLayout();
			// 
			// ucTricks1
			// 
			this.ucTricks1.Location = new System.Drawing.Point(8, 350);
			this.ucTricks1.Name = "ucTricks1";
			this.ucTricks1.Size = new System.Drawing.Size(203, 301);
			this.ucTricks1.TabIndex = 5;
			// 
			// ucSaySelector1
			// 
			this.ucSaySelector1.Location = new System.Drawing.Point(4, 264);
			this.ucSaySelector1.Name = "ucSaySelector1";
			this.ucSaySelector1.Size = new System.Drawing.Size(207, 447);
			this.ucSaySelector1.TabIndex = 4;
			// 
			// ucPlayerOverview1
			// 
			this.ucPlayerOverview1.Location = new System.Drawing.Point(4, 12);
			this.ucPlayerOverview1.Name = "ucPlayerOverview1";
			this.ucPlayerOverview1.Size = new System.Drawing.Size(206, 246);
			this.ucPlayerOverview1.TabIndex = 6;
			// 
			// FrmGameInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(221, 742);
			this.ControlBox = false;
			this.Controls.Add(this.ucPlayerOverview1);
			this.Controls.Add(this.ucTricks1);
			this.Controls.Add(this.ucSaySelector1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Info & Options";
			this.ResumeLayout(false);

			}

		#endregion

		private UserControls.UcSaySelector ucSaySelector1;
		private UserControls.UcTricks ucTricks1;
		private UserControls.UcPlayerOverview ucPlayerOverview1;
		}
	}