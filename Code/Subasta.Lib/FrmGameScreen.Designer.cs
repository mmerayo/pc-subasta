namespace Subasta.Lib
{
	partial class FrmGameScreen
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
			this.ucDeclarationSelector1 = new Subasta.Lib.UserControls.UcDeclarationSelector();
			this.ucTrumpSelector = new Subasta.Lib.UserControls.UcTrumpSelection();
			this.gameSets = new Subasta.Lib.UserControls.UcGameSets();
			this.ucTable1 = new Subasta.Lib.UserControls.UcTable();
			this.ucPlayerOverview1 = new Subasta.Lib.UserControls.UcPlayerOverview();
			this.ucTricks1 = new Subasta.Lib.UserControls.UcTricks();
			this.ucSaySelector1 = new Subasta.Lib.UserControls.UcSaySelector();
			this.SuspendLayout();
			// 
			// ucDeclarationSelector1
			// 
			this.ucDeclarationSelector1.Location = new System.Drawing.Point(4, 402);
			this.ucDeclarationSelector1.Name = "ucDeclarationSelector1";
			this.ucDeclarationSelector1.Size = new System.Drawing.Size(192, 88);
			this.ucDeclarationSelector1.TabIndex = 11;
			// 
			// ucTrumpSelector
			// 
			this.ucTrumpSelector.Location = new System.Drawing.Point(4, 402);
			this.ucTrumpSelector.Name = "ucTrumpSelector";
			this.ucTrumpSelector.Size = new System.Drawing.Size(206, 81);
			this.ucTrumpSelector.TabIndex = 10;
			// 
			// gameSets
			// 
			this.gameSets.Location = new System.Drawing.Point(0, 0);
			this.gameSets.Name = "gameSets";
			this.gameSets.Size = new System.Drawing.Size(206, 395);
			this.gameSets.TabIndex = 9;
			// 
			// ucTable1
			// 
			this.ucTable1.BackColor = System.Drawing.Color.ForestGreen;
			this.ucTable1.Location = new System.Drawing.Point(213, 0);
			this.ucTable1.Name = "ucTable1";
			this.ucTable1.Size = new System.Drawing.Size(720, 720);
			this.ucTable1.TabIndex = 12;
			// 
			// ucPlayerOverview1
			// 
			this.ucPlayerOverview1.Location = new System.Drawing.Point(939, 0);
			this.ucPlayerOverview1.Name = "ucPlayerOverview1";
			this.ucPlayerOverview1.Size = new System.Drawing.Size(206, 246);
			this.ucPlayerOverview1.TabIndex = 15;
			// 
			// ucTricks1
			// 
			this.ucTricks1.Location = new System.Drawing.Point(943, 338);
			this.ucTricks1.Name = "ucTricks1";
			this.ucTricks1.Size = new System.Drawing.Size(203, 301);
			this.ucTricks1.TabIndex = 14;
			// 
			// ucSaySelector1
			// 
			this.ucSaySelector1.Location = new System.Drawing.Point(939, 252);
			this.ucSaySelector1.Name = "ucSaySelector1";
			this.ucSaySelector1.Size = new System.Drawing.Size(207, 447);
			this.ucSaySelector1.TabIndex = 13;
			// 
			// FrmGameScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1155, 720);
			this.Controls.Add(this.ucPlayerOverview1);
			this.Controls.Add(this.ucTricks1);
			this.Controls.Add(this.ucSaySelector1);
			this.Controls.Add(this.ucTable1);
			this.Controls.Add(this.ucDeclarationSelector1);
			this.Controls.Add(this.ucTrumpSelector);
			this.Controls.Add(this.gameSets);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameScreen";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "FrmGameScreen";
			this.ResumeLayout(false);

		}

		#endregion

		private UserControls.UcDeclarationSelector ucDeclarationSelector1;
		private UserControls.UcTrumpSelection ucTrumpSelector;
		private UserControls.UcGameSets gameSets;
		private UserControls.UcTable ucTable1;
		private UserControls.UcPlayerOverview ucPlayerOverview1;
		private UserControls.UcTricks ucTricks1;
		private UserControls.UcSaySelector ucSaySelector1;
	}
}