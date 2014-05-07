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
		this.grpPuntos24 = new System.Windows.Forms.GroupBox();
		this.lblPuntos24 = new System.Windows.Forms.Label();
		this.grpPtos13 = new System.Windows.Forms.GroupBox();
		this.lblPuntos13 = new System.Windows.Forms.Label();
		this.grpPuntos24.SuspendLayout();
		this.grpPtos13.SuspendLayout();
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
		this.txtBazas.Size = new System.Drawing.Size(197, 183);
		this.txtBazas.TabIndex = 5;
		// 
		// grpPuntos24
		// 
		this.grpPuntos24.Controls.Add(this.lblPuntos24);
		this.grpPuntos24.Location = new System.Drawing.Point(0, 239);
		this.grpPuntos24.Name = "grpPuntos24";
		this.grpPuntos24.Size = new System.Drawing.Size(197, 51);
		this.grpPuntos24.TabIndex = 7;
		this.grpPuntos24.TabStop = false;
		this.grpPuntos24.Text = "Puntos J2/J4";
		// 
		// lblPuntos24
		// 
		this.lblPuntos24.AutoSize = true;
		this.lblPuntos24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblPuntos24.Location = new System.Drawing.Point(6, 16);
		this.lblPuntos24.Name = "lblPuntos24";
		this.lblPuntos24.Size = new System.Drawing.Size(51, 20);
		this.lblPuntos24.TabIndex = 0;
		this.lblPuntos24.Text = "label1";
		// 
		// grpPtos13
		// 
		this.grpPtos13.Controls.Add(this.lblPuntos13);
		this.grpPtos13.Location = new System.Drawing.Point(0, 182);
		this.grpPtos13.Name = "grpPtos13";
		this.grpPtos13.Size = new System.Drawing.Size(197, 51);
		this.grpPtos13.TabIndex = 6;
		this.grpPtos13.TabStop = false;
		this.grpPtos13.Text = "Puntos J1/J3";
		// 
		// lblPuntos13
		// 
		this.lblPuntos13.AutoSize = true;
		this.lblPuntos13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblPuntos13.Location = new System.Drawing.Point(6, 16);
		this.lblPuntos13.Name = "lblPuntos13";
		this.lblPuntos13.Size = new System.Drawing.Size(51, 20);
		this.lblPuntos13.TabIndex = 0;
		this.lblPuntos13.Text = "label1";
		// 
		// UcTricks
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.grpPuntos24);
		this.Controls.Add(this.grpPtos13);
		this.Controls.Add(this.txtBazas);
		this.Name = "UcTricks";
		this.Size = new System.Drawing.Size(203, 289);
		this.grpPuntos24.ResumeLayout(false);
		this.grpPuntos24.PerformLayout();
		this.grpPtos13.ResumeLayout(false);
		this.grpPtos13.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBazas;
		private System.Windows.Forms.GroupBox grpPuntos24;
		private System.Windows.Forms.Label lblPuntos24;
		private System.Windows.Forms.GroupBox grpPtos13;
		private System.Windows.Forms.Label lblPuntos13;
	}
}
