namespace Subasta.Lib.UserControls
{
	partial class UcTrumpSelection
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpTrumpOptions = new System.Windows.Forms.GroupBox();
			this.btnSelectTrump = new System.Windows.Forms.Button();
			this.cmbSuits = new System.Windows.Forms.ComboBox();
			this.grpTrumpOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpTrumpOptions
			// 
			this.grpTrumpOptions.Controls.Add(this.btnSelectTrump);
			this.grpTrumpOptions.Controls.Add(this.cmbSuits);
			this.grpTrumpOptions.Location = new System.Drawing.Point(0, 0);
			this.grpTrumpOptions.Name = "grpTrumpOptions";
			this.grpTrumpOptions.Size = new System.Drawing.Size(206, 81);
			this.grpTrumpOptions.TabIndex = 5;
			this.grpTrumpOptions.TabStop = false;
			this.grpTrumpOptions.Text = "Seleccion de triunfo";
			// 
			// btnSelectTrump
			// 
			this.btnSelectTrump.Location = new System.Drawing.Point(41, 47);
			this.btnSelectTrump.Name = "btnSelectTrump";
			this.btnSelectTrump.Size = new System.Drawing.Size(115, 28);
			this.btnSelectTrump.TabIndex = 1;
			this.btnSelectTrump.Text = "Seleccionar";
			this.btnSelectTrump.UseVisualStyleBackColor = true;
			this.btnSelectTrump.Click += new System.EventHandler(this.btnSelectTrump_Click);
			// 
			// cmbSuits
			// 
			this.cmbSuits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSuits.FormattingEnabled = true;
			this.cmbSuits.Location = new System.Drawing.Point(41, 19);
			this.cmbSuits.Name = "cmbSuits";
			this.cmbSuits.Size = new System.Drawing.Size(115, 21);
			this.cmbSuits.TabIndex = 0;
			// 
			// UcTrumpSelection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpTrumpOptions);
			this.Name = "UcTrumpSelection";
			this.Size = new System.Drawing.Size(206, 81);
			this.grpTrumpOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpTrumpOptions;
		private System.Windows.Forms.Button btnSelectTrump;
		private System.Windows.Forms.ComboBox cmbSuits;
	}
}
