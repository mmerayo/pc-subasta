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
		this.ucCurrentHandInfo1 = new Subasta.Lib.UserControls.UcCurrentHandInfo();
		this.ucUserInteractionContainer1 = new Subasta.Lib.UserControls.UcUserInteractionContainer();
		this.ucPlayerOverview1 = new Subasta.Lib.UserControls.UcPlayerOverview();
		this.ucTricks1 = new Subasta.Lib.UserControls.UcTricks();
		this.ucTable1 = new Subasta.Lib.UserControls.UcTable();
		this.gameSets = new Subasta.Lib.UserControls.UcGameSets();
		this.SuspendLayout();
		// 
		// ucCurrentHandInfo1
		// 
		this.ucCurrentHandInfo1.Location = new System.Drawing.Point(941, 247);
		this.ucCurrentHandInfo1.Name = "ucCurrentHandInfo1";
		this.ucCurrentHandInfo1.Size = new System.Drawing.Size(214, 257);
		this.ucCurrentHandInfo1.TabIndex = 17;
		this.ucCurrentHandInfo1.Visible = false;
		// 
		// ucUserInteractionContainer1
		// 
		this.ucUserInteractionContainer1.Location = new System.Drawing.Point(939, 247);
		this.ucUserInteractionContainer1.Name = "ucUserInteractionContainer1";
		this.ucUserInteractionContainer1.Size = new System.Drawing.Size(216, 473);
		this.ucUserInteractionContainer1.TabIndex = 16;
		// 
		// ucPlayerOverview1
		// 
		this.ucPlayerOverview1.Location = new System.Drawing.Point(939, 0);
		this.ucPlayerOverview1.Name = "ucPlayerOverview1";
		this.ucPlayerOverview1.Size = new System.Drawing.Size(216, 246);
		this.ucPlayerOverview1.TabIndex = 15;
		// 
		// ucTricks1
		// 
		this.ucTricks1.Location = new System.Drawing.Point(4, 273);
		this.ucTricks1.Name = "ucTricks1";
		this.ucTricks1.Size = new System.Drawing.Size(203, 338);
		this.ucTricks1.TabIndex = 14;
		this.ucTricks1.Visible = false;
		// 
		// ucTable1
		// 
		this.ucTable1.BackColor = System.Drawing.Color.ForestGreen;
		this.ucTable1.Location = new System.Drawing.Point(213, 0);
		this.ucTable1.Name = "ucTable1";
		this.ucTable1.Size = new System.Drawing.Size(720, 720);
		this.ucTable1.TabIndex = 12;
		// 
		// gameSets
		// 
		this.gameSets.Location = new System.Drawing.Point(0, 0);
		this.gameSets.Name = "gameSets";
		this.gameSets.Size = new System.Drawing.Size(206, 258);
		this.gameSets.TabIndex = 9;
		// 
		// FrmGameScreen
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(1155, 720);
		this.Controls.Add(this.ucCurrentHandInfo1);
		this.Controls.Add(this.ucUserInteractionContainer1);
		this.Controls.Add(this.ucPlayerOverview1);
		this.Controls.Add(this.ucTricks1);
		this.Controls.Add(this.ucTable1);
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

		private UserControls.UcGameSets gameSets;
		private UserControls.UcTable ucTable1;
		private UserControls.UcPlayerOverview ucPlayerOverview1;
		private UserControls.UcTricks ucTricks1;
		private UserControls.UcUserInteractionContainer ucUserInteractionContainer1;
		private UserControls.UcCurrentHandInfo ucCurrentHandInfo1;
	}
}