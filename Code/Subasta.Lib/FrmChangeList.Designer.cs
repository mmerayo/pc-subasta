namespace Subasta.Lib
	{
	partial class FrmChangeList
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
			this.txtChanges = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtChanges
			// 
			this.txtChanges.Location = new System.Drawing.Point(13, 13);
			this.txtChanges.Multiline = true;
			this.txtChanges.Name = "txtChanges";
			this.txtChanges.ReadOnly = true;
			this.txtChanges.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtChanges.Size = new System.Drawing.Size(697, 237);
			this.txtChanges.TabIndex = 0;
			// 
			// FrmChangeList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722, 265);
			this.Controls.Add(this.txtChanges);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmChangeList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Registro de actualizaciones";
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.TextBox txtChanges;
		}
	}