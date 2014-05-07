namespace Subasta.Lib
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
			this.grpPtos13 = new System.Windows.Forms.GroupBox();
			this.lblPuntos13 = new System.Windows.Forms.Label();
			this.grpPuntos24 = new System.Windows.Forms.GroupBox();
			this.lblPuntos24 = new System.Windows.Forms.Label();
			this.grpFirstPlayer = new System.Windows.Forms.GroupBox();
			this.txtBazas = new System.Windows.Forms.TextBox();
			this.lblFirstPlayer = new System.Windows.Forms.Label();
			this.grpPlayerBets = new System.Windows.Forms.GroupBox();
			this.lblPlayerBets = new System.Windows.Forms.Label();
			this.grpPuntos = new System.Windows.Forms.GroupBox();
			this.lblPuntos = new System.Windows.Forms.Label();
			this.ucSaySelector1 = new Subasta.Lib.UserControls.UcSaySelector();
			this.grpPlayerTurn.SuspendLayout();
			this.grpTrump.SuspendLayout();
			this.grpPtos13.SuspendLayout();
			this.grpPuntos24.SuspendLayout();
			this.grpFirstPlayer.SuspendLayout();
			this.grpPlayerBets.SuspendLayout();
			this.grpPuntos.SuspendLayout();
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
			// grpPtos13
			// 
			this.grpPtos13.Controls.Add(this.lblPuntos13);
			this.grpPtos13.Location = new System.Drawing.Point(14, 236);
			this.grpPtos13.Name = "grpPtos13";
			this.grpPtos13.Size = new System.Drawing.Size(197, 51);
			this.grpPtos13.TabIndex = 2;
			this.grpPtos13.TabStop = false;
			this.grpPtos13.Text = "Puntos J1/J3";
			this.grpPtos13.Visible = false;
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
			// grpPuntos24
			// 
			this.grpPuntos24.Controls.Add(this.lblPuntos24);
			this.grpPuntos24.Location = new System.Drawing.Point(14, 293);
			this.grpPuntos24.Name = "grpPuntos24";
			this.grpPuntos24.Size = new System.Drawing.Size(197, 51);
			this.grpPuntos24.TabIndex = 2;
			this.grpPuntos24.TabStop = false;
			this.grpPuntos24.Text = "Puntos J2/J4";
			this.grpPuntos24.Visible = false;
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
			// grpFirstPlayer
			// 
			this.grpFirstPlayer.Controls.Add(this.txtBazas);
			this.grpFirstPlayer.Controls.Add(this.lblFirstPlayer);
			this.grpFirstPlayer.Location = new System.Drawing.Point(14, 65);
			this.grpFirstPlayer.Name = "grpFirstPlayer";
			this.grpFirstPlayer.Size = new System.Drawing.Size(195, 51);
			this.grpFirstPlayer.TabIndex = 2;
			this.grpFirstPlayer.TabStop = false;
			this.grpFirstPlayer.Text = "Jugador sale";
			// 
			// txtBazas
			// 
			this.txtBazas.Location = new System.Drawing.Point(-2, -35);
			this.txtBazas.Multiline = true;
			this.txtBazas.Name = "txtBazas";
			this.txtBazas.ReadOnly = true;
			this.txtBazas.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtBazas.Size = new System.Drawing.Size(197, 150);
			this.txtBazas.TabIndex = 4;
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
			// ucSaySelector1
			// 
			this.ucSaySelector1.Location = new System.Drawing.Point(4, 179);
			this.ucSaySelector1.Name = "ucSaySelector1";
			this.ucSaySelector1.Size = new System.Drawing.Size(207, 447);
			this.ucSaySelector1.TabIndex = 4;
			// 
			// FrmGameInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(221, 677);
			this.ControlBox = false;
			this.Controls.Add(this.ucSaySelector1);
			this.Controls.Add(this.grpPuntos);
			this.Controls.Add(this.grpPlayerBets);
			this.Controls.Add(this.grpFirstPlayer);
			this.Controls.Add(this.grpPuntos24);
			this.Controls.Add(this.grpPtos13);
			this.Controls.Add(this.grpTrump);
			this.Controls.Add(this.grpPlayerTurn);
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
			this.grpPtos13.ResumeLayout(false);
			this.grpPtos13.PerformLayout();
			this.grpPuntos24.ResumeLayout(false);
			this.grpPuntos24.PerformLayout();
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
		private System.Windows.Forms.GroupBox grpPtos13;
		private System.Windows.Forms.Label lblPuntos13;
		private System.Windows.Forms.GroupBox grpPuntos24;
		private System.Windows.Forms.Label lblPuntos24;
		private System.Windows.Forms.GroupBox grpFirstPlayer;
		private System.Windows.Forms.Label lblFirstPlayer;
		private System.Windows.Forms.GroupBox grpPlayerBets;
		private System.Windows.Forms.Label lblPlayerBets;
		private System.Windows.Forms.GroupBox grpPuntos;
		private System.Windows.Forms.Label lblPuntos;
		private System.Windows.Forms.TextBox txtBazas;
		private UserControls.UcSaySelector ucSaySelector1;
		}
	}