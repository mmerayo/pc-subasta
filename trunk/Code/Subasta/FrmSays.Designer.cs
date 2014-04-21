namespace Subasta
	{
	partial class FrmSays
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblSale = new System.Windows.Forms.Label();
			this.dgvMarques = new System.Windows.Forms.DataGridView();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.optionsListBox = new System.Windows.Forms.CheckedListBox();
			this.btnMarcar = new System.Windows.Forms.Button();
			this.pbCard = new System.Windows.Forms.PictureBox();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.grpCards = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvMarques)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbCard)).BeginInit();
			this.grpCards.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dgvMarques);
			this.groupBox1.Controls.Add(this.lblSale);
			this.groupBox1.Location = new System.Drawing.Point(10, 108);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 250);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Estado";
			// 
			// lblSale
			// 
			this.lblSale.AutoSize = true;
			this.lblSale.Location = new System.Drawing.Point(6, 33);
			this.lblSale.Name = "lblSale";
			this.lblSale.Size = new System.Drawing.Size(35, 13);
			this.lblSale.TabIndex = 0;
			this.lblSale.Text = "label1";
			// 
			// dgvMarques
			// 
			this.dgvMarques.AllowUserToAddRows = false;
			this.dgvMarques.AllowUserToDeleteRows = false;
			this.dgvMarques.AllowUserToOrderColumns = true;
			this.dgvMarques.AllowUserToResizeColumns = false;
			this.dgvMarques.AllowUserToResizeRows = false;
			this.dgvMarques.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvMarques.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgvMarques.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMarques.Location = new System.Drawing.Point(9, 64);
			this.dgvMarques.Name = "dgvMarques";
			this.dgvMarques.Size = new System.Drawing.Size(245, 180);
			this.dgvMarques.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnMarcar);
			this.groupBox2.Controls.Add(this.optionsListBox);
			this.groupBox2.Location = new System.Drawing.Point(284, 108);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(344, 250);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Marcar";
			// 
			// optionsListBox
			// 
			this.optionsListBox.BackColor = System.Drawing.SystemColors.Control;
			this.optionsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.optionsListBox.FormattingEnabled = true;
			this.optionsListBox.Location = new System.Drawing.Point(16, 19);
			this.optionsListBox.Name = "optionsListBox";
			this.optionsListBox.Size = new System.Drawing.Size(142, 210);
			this.optionsListBox.TabIndex = 0;
			// 
			// btnMarcar
			// 
			this.btnMarcar.Location = new System.Drawing.Point(241, 75);
			this.btnMarcar.Name = "btnMarcar";
			this.btnMarcar.Size = new System.Drawing.Size(75, 52);
			this.btnMarcar.TabIndex = 1;
			this.btnMarcar.Text = "Marcar";
			this.btnMarcar.UseVisualStyleBackColor = true;
			// 
			// pbCard
			// 
			this.pbCard.Location = new System.Drawing.Point(23, 14);
			this.pbCard.Name = "pbCard";
			this.pbCard.Size = new System.Drawing.Size(50, 70);
			this.pbCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbCard.TabIndex = 2;
			this.pbCard.TabStop = false;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// grpCards
			// 
			this.grpCards.Controls.Add(this.pbCard);
			this.grpCards.Location = new System.Drawing.Point(12, 12);
			this.grpCards.Name = "grpCards";
			this.grpCards.Size = new System.Drawing.Size(616, 90);
			this.grpCards.TabIndex = 3;
			this.grpCards.TabStop = false;
			this.grpCards.Text = "Tus cartas";
			// 
			// FrmSays
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 370);
			this.Controls.Add(this.grpCards);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FrmSays";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Marque";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvMarques)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbCard)).EndInit();
			this.grpCards.ResumeLayout(false);
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblSale;
		private System.Windows.Forms.DataGridView dgvMarques;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnMarcar;
		private System.Windows.Forms.CheckedListBox optionsListBox;
		private System.Windows.Forms.PictureBox pbCard;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.GroupBox grpCards;
		}
	}