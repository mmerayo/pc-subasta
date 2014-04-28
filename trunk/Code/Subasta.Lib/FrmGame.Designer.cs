﻿namespace Subasta.Lib
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
			((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
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
			// FrmGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.ForestGreen;
			this.ClientSize = new System.Drawing.Size(720, 720);
			this.ControlBox = false;
			this.Controls.Add(this.pb1);
			this.Controls.Add(this.pb3);
			this.Controls.Add(this.pb4);
			this.Controls.Add(this.pb2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGame";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Juego";
			((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.PictureBox pb2;
		private System.Windows.Forms.PictureBox pb4;
		private System.Windows.Forms.PictureBox pb3;
		private System.Windows.Forms.PictureBox pb1;
		private System.Windows.Forms.ToolTip balloonInfo;
		}
	}