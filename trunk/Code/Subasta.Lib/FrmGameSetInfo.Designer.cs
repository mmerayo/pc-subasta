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
			this.txtGameSetStatus = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtGameSetStatus
			// 
			this.txtGameSetStatus.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGameSetStatus.Location = new System.Drawing.Point(13, 13);
			this.txtGameSetStatus.Multiline = true;
			this.txtGameSetStatus.Name = "txtGameSetStatus";
			this.txtGameSetStatus.ReadOnly = true;
			this.txtGameSetStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGameSetStatus.Size = new System.Drawing.Size(187, 381);
			this.txtGameSetStatus.TabIndex = 0;
			// 
			// FrmGameSetInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(211, 417);
			this.ControlBox = false;
			this.Controls.Add(this.txtGameSetStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameSetInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Partida";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtGameSetStatus;
	}
}