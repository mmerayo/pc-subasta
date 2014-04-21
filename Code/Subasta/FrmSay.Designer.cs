namespace Subasta
	{
	partial class FrmSay
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
			this.grpPlayerTurn = new System.Windows.Forms.GroupBox();
			this.lblTurn = new System.Windows.Forms.Label();
			this.grpPlayerTurn.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpPlayerTurn
			// 
			this.grpPlayerTurn.Controls.Add(this.lblTurn);
			this.grpPlayerTurn.Location = new System.Drawing.Point(12, 12);
			this.grpPlayerTurn.Name = "grpPlayerTurn";
			this.grpPlayerTurn.Size = new System.Drawing.Size(197, 47);
			this.grpPlayerTurn.TabIndex = 0;
			this.grpPlayerTurn.TabStop = false;
			this.grpPlayerTurn.Text = "Turno";
			// 
			// lblTurn
			// 
			this.lblTurn.AutoSize = true;
			this.lblTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTurn.Location = new System.Drawing.Point(6, 16);
			this.lblTurn.Name = "lblTurn";
			this.lblTurn.Size = new System.Drawing.Size(56, 20);
			this.lblTurn.TabIndex = 0;
			this.lblTurn.Text = "lblTurn";
			// 
			// FrmSay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(221, 317);
			this.ControlBox = false;
			this.Controls.Add(this.grpPlayerTurn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FrmSay";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Info & Options";
			this.Load += new System.EventHandler(this.FrmSay_Load);
			this.VisibleChanged += new System.EventHandler(this.FrmSay_VisibleChanged);
			this.grpPlayerTurn.ResumeLayout(false);
			this.grpPlayerTurn.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.GroupBox grpPlayerTurn;
		private System.Windows.Forms.Label lblTurn;
		}
	}