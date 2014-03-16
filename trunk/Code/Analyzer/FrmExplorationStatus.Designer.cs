namespace Analyzer
	{
	partial class FrmExplorationStatus
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.grpStatus = new System.Windows.Forms.GroupBox();
			this.lblTrump = new System.Windows.Forms.Label();
			this.lblFirstPlayer = new System.Windows.Forms.Label();
			this.labelDepth = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.imageListCards = new System.Windows.Forms.ImageList(this.components);
			this.lblPlayerBets = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.grpStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
			this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(35, 359);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(516, 242);
			this.dataGridView1.TabIndex = 0;
			// 
			// grpStatus
			// 
			this.grpStatus.Controls.Add(this.lblPlayerBets);
			this.grpStatus.Controls.Add(this.lblTrump);
			this.grpStatus.Controls.Add(this.lblFirstPlayer);
			this.grpStatus.Controls.Add(this.labelDepth);
			this.grpStatus.Controls.Add(this.label4);
			this.grpStatus.Controls.Add(this.label3);
			this.grpStatus.Controls.Add(this.label2);
			this.grpStatus.Controls.Add(this.label1);
			this.grpStatus.Location = new System.Drawing.Point(35, 13);
			this.grpStatus.Name = "grpStatus";
			this.grpStatus.Size = new System.Drawing.Size(682, 318);
			this.grpStatus.TabIndex = 1;
			this.grpStatus.TabStop = false;
			this.grpStatus.Text = "Initial status";
			// 
			// lblTrump
			// 
			this.lblTrump.AutoSize = true;
			this.lblTrump.Location = new System.Drawing.Point(417, 85);
			this.lblTrump.Name = "lblTrump";
			this.lblTrump.Size = new System.Drawing.Size(47, 13);
			this.lblTrump.TabIndex = 6;
			this.lblTrump.Text = "lblTrump";
			// 
			// lblFirstPlayer
			// 
			this.lblFirstPlayer.AutoSize = true;
			this.lblFirstPlayer.Location = new System.Drawing.Point(417, 56);
			this.lblFirstPlayer.Name = "lblFirstPlayer";
			this.lblFirstPlayer.Size = new System.Drawing.Size(65, 13);
			this.lblFirstPlayer.TabIndex = 5;
			this.lblFirstPlayer.Text = "lblFirstPlayer";
			// 
			// labelDepth
			// 
			this.labelDepth.AutoSize = true;
			this.labelDepth.Location = new System.Drawing.Point(417, 30);
			this.labelDepth.Name = "labelDepth";
			this.labelDepth.Size = new System.Drawing.Size(91, 13);
			this.labelDepth.TabIndex = 4;
			this.labelDepth.Text = "Exploration Depth";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 248);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Player 4:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 183);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Player 3:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 109);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Player 2:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Player 1:";
			// 
			// imageListCards
			// 
			this.imageListCards.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListCards.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListCards.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// lblPlayerBets
			// 
			this.lblPlayerBets.AutoSize = true;
			this.lblPlayerBets.Location = new System.Drawing.Point(417, 109);
			this.lblPlayerBets.Name = "lblPlayerBets";
			this.lblPlayerBets.Size = new System.Drawing.Size(67, 13);
			this.lblPlayerBets.TabIndex = 7;
			this.lblPlayerBets.Text = "lblPlayerBets";
			// 
			// FrmExplorationStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(766, 668);
			this.Controls.Add(this.grpStatus);
			this.Controls.Add(this.dataGridView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmExplorationStatus";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "FrmExplorationStatus";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.grpStatus.ResumeLayout(false);
			this.grpStatus.PerformLayout();
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.GroupBox grpStatus;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ImageList imageListCards;
		private System.Windows.Forms.Label labelDepth;
		private System.Windows.Forms.Label lblTrump;
		private System.Windows.Forms.Label lblFirstPlayer;
		private System.Windows.Forms.Label lblPlayerBets;
		}
	}