namespace Subasta.Lib.UserControls
{
	partial class UcTable
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.pbPetar = new System.Windows.Forms.PictureBox();
			this.lblInfo = new System.Windows.Forms.Label();
			this.pb2 = new System.Windows.Forms.PictureBox();
			this.balloonInfo = new System.Windows.Forms.ToolTip(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.pb1 = new System.Windows.Forms.PictureBox();
			this.pb3 = new System.Windows.Forms.PictureBox();
			this.pb4 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbPetar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.DarkKhaki;
			this.label1.Location = new System.Drawing.Point(-4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(205, 20);
			this.label1.TabIndex = 14;
			this.label1.Text = "Subasta (Alpha Version)";
			// 
			// pbPetar
			// 
			this.pbPetar.BackColor = System.Drawing.Color.Transparent;
			this.pbPetar.Location = new System.Drawing.Point(332, 326);
			this.pbPetar.Name = "pbPetar";
			this.pbPetar.Size = new System.Drawing.Size(40, 40);
			this.pbPetar.TabIndex = 13;
			this.pbPetar.TabStop = false;
			this.pbPetar.Visible = false;
			// 
			// lblInfo
			// 
			this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Maroon;
			this.lblInfo.Location = new System.Drawing.Point(-3, 20);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(210, 37);
			this.lblInfo.TabIndex = 12;
			this.lblInfo.Text = "Use el boton derecho del ratón para petar en la mesa";
			this.lblInfo.Visible = false;
			// 
			// pb2
			// 
			this.pb2.BackColor = System.Drawing.Color.Transparent;
			this.pb2.Location = new System.Drawing.Point(622, 232);
			this.pb2.Name = "pb2";
			this.pb2.Size = new System.Drawing.Size(40, 40);
			this.pb2.TabIndex = 8;
			this.pb2.TabStop = false;
			this.pb2.Visible = false;
			// 
			// balloonInfo
			// 
			this.balloonInfo.AutomaticDelay = 1000;
			this.balloonInfo.AutoPopDelay = 1000;
			this.balloonInfo.InitialDelay = 0;
			this.balloonInfo.IsBalloon = true;
			this.balloonInfo.ReshowDelay = 0;
			this.balloonInfo.ShowAlways = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.DarkKhaki;
			this.label2.Location = new System.Drawing.Point(479, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(239, 16);
			this.label2.TabIndex = 15;
			this.label2.Text = "Resolución recomendada >= 1280x800";
			// 
			// pb1
			// 
			this.pb1.BackColor = System.Drawing.Color.Transparent;
			this.pb1.Location = new System.Drawing.Point(332, 518);
			this.pb1.Name = "pb1";
			this.pb1.Size = new System.Drawing.Size(40, 40);
			this.pb1.TabIndex = 11;
			this.pb1.TabStop = false;
			this.pb1.Visible = false;
			// 
			// pb3
			// 
			this.pb3.BackColor = System.Drawing.Color.Transparent;
			this.pb3.Location = new System.Drawing.Point(307, 0);
			this.pb3.Name = "pb3";
			this.pb3.Size = new System.Drawing.Size(40, 40);
			this.pb3.TabIndex = 10;
			this.pb3.TabStop = false;
			this.pb3.Visible = false;
			// 
			// pb4
			// 
			this.pb4.BackColor = System.Drawing.Color.Transparent;
			this.pb4.Location = new System.Drawing.Point(0, 232);
			this.pb4.Name = "pb4";
			this.pb4.Size = new System.Drawing.Size(40, 40);
			this.pb4.TabIndex = 9;
			this.pb4.TabStop = false;
			this.pb4.Visible = false;
			// 
			// UcTable
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.ForestGreen;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbPetar);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.pb2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pb1);
			this.Controls.Add(this.pb3);
			this.Controls.Add(this.pb4);
			this.Name = "UcTable";
			this.Size = new System.Drawing.Size(720, 720);
			((System.ComponentModel.ISupportInitialize)(this.pbPetar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbPetar;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.PictureBox pb2;
		private System.Windows.Forms.ToolTip balloonInfo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pb1;
		private System.Windows.Forms.PictureBox pb3;
		private System.Windows.Forms.PictureBox pb4;
	}
}
