namespace Subasta.Lib.UserControls
{
	partial class UcTricks
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
			this.txtBazas = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtBazas
			// 
			this.txtBazas.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.txtBazas.Location = new System.Drawing.Point(0, 0);
			this.txtBazas.Multiline = true;
			this.txtBazas.Name = "txtBazas";
			this.txtBazas.ReadOnly = true;
			this.txtBazas.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtBazas.Size = new System.Drawing.Size(197, 166);
			this.txtBazas.TabIndex = 5;
			// 
			// UcTricks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtBazas);
			this.Name = "UcTricks";
			this.Size = new System.Drawing.Size(197, 166);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBazas;
	}
}
