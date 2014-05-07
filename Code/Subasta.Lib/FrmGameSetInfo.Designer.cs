namespace Subasta.Lib
{
	partial class FrmGameSetInfo
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
			this.gameSets = new Subasta.Lib.UserControls.UcGameSets();
			this.ucTrumpSelector = new Subasta.Lib.UserControls.UcTrumpSelection();
			this.ucDeclarationSelector1 = new Subasta.Lib.UserControls.UcDeclarationSelector();
			this.SuspendLayout();
			// 
			// gameSets
			// 
			this.gameSets.Location = new System.Drawing.Point(2, 4);
			this.gameSets.Name = "gameSets";
			this.gameSets.Size = new System.Drawing.Size(206, 395);
			this.gameSets.TabIndex = 6;
			// 
			// ucTrumpSelector
			// 
			this.ucTrumpSelector.Location = new System.Drawing.Point(5, 405);
			this.ucTrumpSelector.Name = "ucTrumpSelector";
			this.ucTrumpSelector.Size = new System.Drawing.Size(206, 81);
			this.ucTrumpSelector.TabIndex = 7;
			// 
			// ucDeclarationSelector1
			// 
			this.ucDeclarationSelector1.Location = new System.Drawing.Point(5, 405);
			this.ucDeclarationSelector1.Name = "ucDeclarationSelector1";
			this.ucDeclarationSelector1.Size = new System.Drawing.Size(192, 88);
			this.ucDeclarationSelector1.TabIndex = 8;
			// 
			// FrmGameSetInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(214, 670);
			this.ControlBox = false;
			this.Controls.Add(this.ucDeclarationSelector1);
			this.Controls.Add(this.ucTrumpSelector);
			this.Controls.Add(this.gameSets);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameSetInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Partida";
			this.ResumeLayout(false);

		}

		#endregion

		private UserControls.UcGameSets gameSets;
		private UserControls.UcTrumpSelection ucTrumpSelection1;
		private UserControls.UcTrumpSelection ucTrumpSelector;
		private UserControls.UcDeclarationSelector ucDeclarationSelector1;
	}
}