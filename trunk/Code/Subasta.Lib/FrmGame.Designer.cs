namespace Subasta.Lib
	{
	partial class FrmGame
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
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.pb2 = new System.Windows.Forms.PictureBox();
			this.pb4 = new System.Windows.Forms.PictureBox();
			this.pb3 = new System.Windows.Forms.PictureBox();
			this.pb1 = new System.Windows.Forms.PictureBox();
			this.balloonInfo = new System.Windows.Forms.ToolTip(this.components);
			this.lblInfo = new System.Windows.Forms.Label();
			this.pbPetar = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPetar)).BeginInit();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// pb2
			// 
			this.pb2.BackColor = System.Drawing.Color.Transparent;
			this.pb2.Location = new System.Drawing.Point(622, 231);
			this.pb2.Name = "pb2";
			this.pb2.Size = new System.Drawing.Size(40, 40);
			this.pb2.TabIndex = 0;
			this.pb2.TabStop = false;
			this.pb2.Visible = false;
			// 
			// pb4
			// 
			this.pb4.BackColor = System.Drawing.Color.Transparent;
			this.pb4.Location = new System.Drawing.Point(0, 231);
			this.pb4.Name = "pb4";
			this.pb4.Size = new System.Drawing.Size(40, 40);
			this.pb4.TabIndex = 1;
			this.pb4.TabStop = false;
			this.pb4.Visible = false;
			// 
			// pb3
			// 
			this.pb3.BackColor = System.Drawing.Color.Transparent;
			this.pb3.Location = new System.Drawing.Point(307, -1);
			this.pb3.Name = "pb3";
			this.pb3.Size = new System.Drawing.Size(40, 40);
			this.pb3.TabIndex = 2;
			this.pb3.TabStop = false;
			this.pb3.Visible = false;
			// 
			// pb1
			// 
			this.pb1.BackColor = System.Drawing.Color.Transparent;
			this.pb1.Location = new System.Drawing.Point(332, 517);
			this.pb1.Name = "pb1";
			this.pb1.Size = new System.Drawing.Size(40, 40);
			this.pb1.TabIndex = 3;
			this.pb1.TabStop = false;
			this.pb1.Visible = false;
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
			// lblInfo
			// 
			this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Maroon;
			this.lblInfo.Location = new System.Drawing.Point(-3, 19);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(210, 37);
			this.lblInfo.TabIndex = 4;
			this.lblInfo.Text = "Use el boton derecho del ratón para petar en la mesa";
			this.lblInfo.Visible = false;
			// 
			// pbPetar
			// 
			this.pbPetar.BackColor = System.Drawing.Color.Transparent;
			this.pbPetar.Location = new System.Drawing.Point(332, 325);
			this.pbPetar.Name = "pbPetar";
			this.pbPetar.Size = new System.Drawing.Size(40, 40);
			this.pbPetar.TabIndex = 5;
			this.pbPetar.TabStop = false;
			this.pbPetar.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.DarkKhaki;
			this.label1.Location = new System.Drawing.Point(-4, -1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(205, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "Subasta (Alpha Version)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.DarkKhaki;
			this.label2.Location = new System.Drawing.Point(479, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(239, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Resolución recomendada >= 1280x800";
			// 
			// FrmGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.ForestGreen;
			this.ClientSize = new System.Drawing.Size(720, 720);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbPetar);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.pb1);
			this.Controls.Add(this.pb3);
			this.Controls.Add(this.pb4);
			this.Controls.Add(this.pb2);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGame";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Juego";
			((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPetar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.PictureBox pb2;
		private System.Windows.Forms.PictureBox pb4;
		private System.Windows.Forms.PictureBox pb3;
		private System.Windows.Forms.PictureBox pb1;
		private System.Windows.Forms.ToolTip balloonInfo;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.PictureBox pbPetar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		}
	}