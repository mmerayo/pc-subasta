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
			this.grpDeclarations = new System.Windows.Forms.GroupBox();
			this.btnDeclarations = new System.Windows.Forms.Button();
			this.cmbDeclarations = new System.Windows.Forms.ComboBox();
			this.gameSets = new Subasta.Lib.UserControls.UcGameSets();
			this.ucTrumpSelector = new Subasta.Lib.UserControls.UcTrumpSelection();
			this.grpDeclarations.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpDeclarations
			// 
			this.grpDeclarations.Controls.Add(this.btnDeclarations);
			this.grpDeclarations.Controls.Add(this.cmbDeclarations);
			this.grpDeclarations.Location = new System.Drawing.Point(10, 405);
			this.grpDeclarations.Name = "grpDeclarations";
			this.grpDeclarations.Size = new System.Drawing.Size(192, 88);
			this.grpDeclarations.TabIndex = 5;
			this.grpDeclarations.TabStop = false;
			this.grpDeclarations.Text = "Cantar";
			// 
			// btnDeclarations
			// 
			this.btnDeclarations.Location = new System.Drawing.Point(12, 46);
			this.btnDeclarations.Name = "btnDeclarations";
			this.btnDeclarations.Size = new System.Drawing.Size(161, 28);
			this.btnDeclarations.TabIndex = 1;
			this.btnDeclarations.Text = "Canta";
			this.btnDeclarations.UseVisualStyleBackColor = true;
			this.btnDeclarations.Click += new System.EventHandler(this.btnDeclarations_Click);
			// 
			// cmbDeclarations
			// 
			this.cmbDeclarations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDeclarations.FormattingEnabled = true;
			this.cmbDeclarations.Location = new System.Drawing.Point(6, 19);
			this.cmbDeclarations.Name = "cmbDeclarations";
			this.cmbDeclarations.Size = new System.Drawing.Size(167, 21);
			this.cmbDeclarations.TabIndex = 0;
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
			// FrmGameSetInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(214, 670);
			this.ControlBox = false;
			this.Controls.Add(this.ucTrumpSelector);
			this.Controls.Add(this.gameSets);
			this.Controls.Add(this.grpDeclarations);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameSetInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Partida";
			this.grpDeclarations.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpDeclarations;
		private System.Windows.Forms.Button btnDeclarations;
		private System.Windows.Forms.ComboBox cmbDeclarations;
		private UserControls.UcGameSets gameSets;
		private UserControls.UcTrumpSelection ucTrumpSelection1;
		private UserControls.UcTrumpSelection ucTrumpSelector;
	}
}