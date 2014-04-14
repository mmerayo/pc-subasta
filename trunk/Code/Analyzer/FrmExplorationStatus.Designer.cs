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
			this.dgvStatus = new System.Windows.Forms.DataGridView();
			this.grpStatus = new System.Windows.Forms.GroupBox();
			this.lblPlayerBets = new System.Windows.Forms.Label();
			this.lblTrump = new System.Windows.Forms.Label();
			this.lblFirstPlayer = new System.Windows.Forms.Label();
			this.labelDepth = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.imageListCards = new System.Windows.Forms.ImageList(this.components);
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dgvSaysStatus = new System.Windows.Forms.DataGridView();
			this.lblDeclarations = new System.Windows.Forms.Label();
			this.lblPointsT2 = new System.Windows.Forms.Label();
			this.lblPointsT1 = new System.Windows.Forms.Label();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
			this.grpStatus.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSaysStatus)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvStatus
			// 
			this.dgvStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
			this.dgvStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStatus.Location = new System.Drawing.Point(3, 198);
			this.dgvStatus.MultiSelect = false;
			this.dgvStatus.Name = "dgvStatus";
			this.dgvStatus.ReadOnly = true;
			this.dgvStatus.Size = new System.Drawing.Size(722, 251);
			this.dgvStatus.TabIndex = 0;
			this.dgvStatus.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
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
			this.grpStatus.Location = new System.Drawing.Point(38, 26);
			this.grpStatus.Name = "grpStatus";
			this.grpStatus.Size = new System.Drawing.Size(682, 318);
			this.grpStatus.TabIndex = 1;
			this.grpStatus.TabStop = false;
			this.grpStatus.Text = "Initial status";
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
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(12, 11);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(742, 580);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgvSaysStatus);
			this.tabPage2.Controls.Add(this.lblDeclarations);
			this.tabPage2.Controls.Add(this.lblPointsT2);
			this.tabPage2.Controls.Add(this.lblPointsT1);
			this.tabPage2.Controls.Add(this.dgvStatus);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(734, 554);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Simulation";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// dgvSaysStatus
			// 
			this.dgvSaysStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSaysStatus.Location = new System.Drawing.Point(3, 3);
			this.dgvSaysStatus.MultiSelect = false;
			this.dgvSaysStatus.Name = "dgvSaysStatus";
			this.dgvSaysStatus.ReadOnly = true;
			this.dgvSaysStatus.Size = new System.Drawing.Size(722, 189);
			this.dgvSaysStatus.TabIndex = 4;
			// 
			// lblDeclarations
			// 
			this.lblDeclarations.AutoSize = true;
			this.lblDeclarations.Location = new System.Drawing.Point(3, 530);
			this.lblDeclarations.Name = "lblDeclarations";
			this.lblDeclarations.Size = new System.Drawing.Size(10, 13);
			this.lblDeclarations.TabIndex = 3;
			this.lblDeclarations.Text = " ";
			// 
			// lblPointsT2
			// 
			this.lblPointsT2.AutoSize = true;
			this.lblPointsT2.Location = new System.Drawing.Point(3, 498);
			this.lblPointsT2.Name = "lblPointsT2";
			this.lblPointsT2.Size = new System.Drawing.Size(10, 13);
			this.lblPointsT2.TabIndex = 2;
			this.lblPointsT2.Text = " ";
			// 
			// lblPointsT1
			// 
			this.lblPointsT1.AutoSize = true;
			this.lblPointsT1.Location = new System.Drawing.Point(3, 464);
			this.lblPointsT1.Name = "lblPointsT1";
			this.lblPointsT1.Size = new System.Drawing.Size(10, 13);
			this.lblPointsT1.TabIndex = 1;
			this.lblPointsT1.Text = " ";
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.grpStatus);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(734, 554);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Configuration";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// FrmExplorationStatus
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(766, 633);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmExplorationStatus";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "FrmExplorationStatus";
			((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).EndInit();
			this.grpStatus.ResumeLayout(false);
			this.grpStatus.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSaysStatus)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.DataGridView dgvStatus;
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
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label lblPointsT2;
		private System.Windows.Forms.Label lblPointsT1;
		private System.Windows.Forms.Label lblDeclarations;
		private System.Windows.Forms.DataGridView dgvSaysStatus;
		}
	}