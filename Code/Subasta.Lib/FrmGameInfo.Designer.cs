﻿namespace Subasta.Lib
	{
	partial class FrmGameInfo
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
			this.grpTrump = new System.Windows.Forms.GroupBox();
			this.lblTrump = new System.Windows.Forms.Label();
			this.grpFirstPlayer = new System.Windows.Forms.GroupBox();
			this.lblFirstPlayer = new System.Windows.Forms.Label();
			this.grpPlayerBets = new System.Windows.Forms.GroupBox();
			this.lblPlayerBets = new System.Windows.Forms.Label();
			this.grpPuntos = new System.Windows.Forms.GroupBox();
			this.lblPuntos = new System.Windows.Forms.Label();
			this.ucTricks1 = new Subasta.Lib.UserControls.UcTricks();
			this.ucSaySelector1 = new Subasta.Lib.UserControls.UcSaySelector();
			this.grpPlayerTurn.SuspendLayout();
			this.grpTrump.SuspendLayout();
			this.grpFirstPlayer.SuspendLayout();
			this.grpPlayerBets.SuspendLayout();
			this.grpPuntos.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpPlayerTurn
			// 
			this.grpPlayerTurn.Controls.Add(this.lblTurn);
			this.grpPlayerTurn.Location = new System.Drawing.Point(14, 12);
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
			// grpTrump
			// 
			this.grpTrump.Controls.Add(this.lblTrump);
			this.grpTrump.Location = new System.Drawing.Point(12, 122);
			this.grpTrump.Name = "grpTrump";
			this.grpTrump.Size = new System.Drawing.Size(197, 51);
			this.grpTrump.TabIndex = 1;
			this.grpTrump.TabStop = false;
			this.grpTrump.Text = "Triunfo";
			this.grpTrump.Visible = false;
			// 
			// lblTrump
			// 
			this.lblTrump.AutoSize = true;
			this.lblTrump.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTrump.Location = new System.Drawing.Point(6, 16);
			this.lblTrump.Name = "lblTrump";
			this.lblTrump.Size = new System.Drawing.Size(51, 20);
			this.lblTrump.TabIndex = 0;
			this.lblTrump.Text = "label1";
			// 
			// grpFirstPlayer
			// 
			this.grpFirstPlayer.Controls.Add(this.lblFirstPlayer);
			this.grpFirstPlayer.Location = new System.Drawing.Point(14, 65);
			this.grpFirstPlayer.Name = "grpFirstPlayer";
			this.grpFirstPlayer.Size = new System.Drawing.Size(195, 51);
			this.grpFirstPlayer.TabIndex = 2;
			this.grpFirstPlayer.TabStop = false;
			this.grpFirstPlayer.Text = "Jugador sale";
			// 
			// lblFirstPlayer
			// 
			this.lblFirstPlayer.AutoSize = true;
			this.lblFirstPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFirstPlayer.Location = new System.Drawing.Point(6, 16);
			this.lblFirstPlayer.Name = "lblFirstPlayer";
			this.lblFirstPlayer.Size = new System.Drawing.Size(51, 20);
			this.lblFirstPlayer.TabIndex = 0;
			this.lblFirstPlayer.Text = "label1";
			// 
			// grpPlayerBets
			// 
			this.grpPlayerBets.Controls.Add(this.lblPlayerBets);
			this.grpPlayerBets.Location = new System.Drawing.Point(12, 179);
			this.grpPlayerBets.Name = "grpPlayerBets";
			this.grpPlayerBets.Size = new System.Drawing.Size(127, 51);
			this.grpPlayerBets.TabIndex = 2;
			this.grpPlayerBets.TabStop = false;
			this.grpPlayerBets.Text = "Jugador apuesta";
			this.grpPlayerBets.Visible = false;
			// 
			// lblPlayerBets
			// 
			this.lblPlayerBets.AutoSize = true;
			this.lblPlayerBets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPlayerBets.Location = new System.Drawing.Point(6, 16);
			this.lblPlayerBets.Name = "lblPlayerBets";
			this.lblPlayerBets.Size = new System.Drawing.Size(51, 20);
			this.lblPlayerBets.TabIndex = 0;
			this.lblPlayerBets.Text = "label1";
			// 
			// grpPuntos
			// 
			this.grpPuntos.Controls.Add(this.lblPuntos);
			this.grpPuntos.Location = new System.Drawing.Point(145, 179);
			this.grpPuntos.Name = "grpPuntos";
			this.grpPuntos.Size = new System.Drawing.Size(56, 51);
			this.grpPuntos.TabIndex = 3;
			this.grpPuntos.TabStop = false;
			this.grpPuntos.Text = "Puntos";
			this.grpPuntos.Visible = false;
			// 
			// lblPuntos
			// 
			this.lblPuntos.AutoSize = true;
			this.lblPuntos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPuntos.Location = new System.Drawing.Point(6, 16);
			this.lblPuntos.Name = "lblPuntos";
			this.lblPuntos.Size = new System.Drawing.Size(51, 20);
			this.lblPuntos.TabIndex = 0;
			this.lblPuntos.Text = "label1";
			// 
			// ucTricks1
			// 
			this.ucTricks1.Location = new System.Drawing.Point(8, 350);
			this.ucTricks1.Name = "ucTricks1";
			this.ucTricks1.Size = new System.Drawing.Size(203, 301);
			this.ucTricks1.TabIndex = 5;
			// 
			// ucSaySelector1
			// 
			this.ucSaySelector1.Location = new System.Drawing.Point(4, 264);
			this.ucSaySelector1.Name = "ucSaySelector1";
			this.ucSaySelector1.Size = new System.Drawing.Size(207, 447);
			this.ucSaySelector1.TabIndex = 4;
			// 
			// FrmGameInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(221, 742);
			this.ControlBox = false;
			this.Controls.Add(this.ucTricks1);
			this.Controls.Add(this.grpPuntos);
			this.Controls.Add(this.grpPlayerBets);
			this.Controls.Add(this.grpFirstPlayer);
			this.Controls.Add(this.grpTrump);
			this.Controls.Add(this.grpPlayerTurn);
			this.Controls.Add(this.ucSaySelector1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Info & Options";
			this.Load += new System.EventHandler(this.FrmSay_Load);
			this.grpPlayerTurn.ResumeLayout(false);
			this.grpPlayerTurn.PerformLayout();
			this.grpTrump.ResumeLayout(false);
			this.grpTrump.PerformLayout();
			this.grpFirstPlayer.ResumeLayout(false);
			this.grpFirstPlayer.PerformLayout();
			this.grpPlayerBets.ResumeLayout(false);
			this.grpPlayerBets.PerformLayout();
			this.grpPuntos.ResumeLayout(false);
			this.grpPuntos.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.GroupBox grpPlayerTurn;
		private System.Windows.Forms.Label lblTurn;
		private System.Windows.Forms.GroupBox grpTrump;
		private System.Windows.Forms.Label lblTrump;
		private System.Windows.Forms.GroupBox grpFirstPlayer;
		private System.Windows.Forms.Label lblFirstPlayer;
		private System.Windows.Forms.GroupBox grpPlayerBets;
		private System.Windows.Forms.Label lblPlayerBets;
		private System.Windows.Forms.GroupBox grpPuntos;
		private System.Windows.Forms.Label lblPuntos;
		private UserControls.UcSaySelector ucSaySelector1;
		private UserControls.UcTricks ucTricks1;
		}
	}