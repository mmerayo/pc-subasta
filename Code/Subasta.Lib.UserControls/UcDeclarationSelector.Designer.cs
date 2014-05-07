namespace Subasta.Lib.UserControls
{
	partial class UcDeclarationSelector
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
			this.grpDeclarations = new System.Windows.Forms.GroupBox();
			this.btnDeclarations = new System.Windows.Forms.Button();
			this.cmbDeclarations = new System.Windows.Forms.ComboBox();
			this.grpDeclarations.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpDeclarations
			// 
			this.grpDeclarations.Controls.Add(this.btnDeclarations);
			this.grpDeclarations.Controls.Add(this.cmbDeclarations);
			this.grpDeclarations.Location = new System.Drawing.Point(0, 0);
			this.grpDeclarations.Name = "grpDeclarations";
			this.grpDeclarations.Size = new System.Drawing.Size(192, 88);
			this.grpDeclarations.TabIndex = 6;
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
			// UcDeclarationSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpDeclarations);
			this.Name = "UcDeclarationSelector";
			this.Size = new System.Drawing.Size(192, 88);
			this.grpDeclarations.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpDeclarations;
		private System.Windows.Forms.Button btnDeclarations;
		private System.Windows.Forms.ComboBox cmbDeclarations;
	}
}
