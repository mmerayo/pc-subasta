namespace Subasta.Lib.UserControls
{
	partial class UcSaySelector
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
			this.grpSayOptions = new System.Windows.Forms.GroupBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.cmbSays = new System.Windows.Forms.ComboBox();
			this.txtMarques = new System.Windows.Forms.TextBox();
			this.txtSays = new System.Windows.Forms.TextBox();
			this.grpSayOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpSayOptions
			// 
			this.grpSayOptions.Controls.Add(this.btnSelect);
			this.grpSayOptions.Controls.Add(this.cmbSays);
			this.grpSayOptions.Location = new System.Drawing.Point(3, 391);
			this.grpSayOptions.Name = "grpSayOptions";
			this.grpSayOptions.Size = new System.Drawing.Size(195, 122);
			this.grpSayOptions.TabIndex = 7;
			this.grpSayOptions.TabStop = false;
			this.grpSayOptions.Text = "Opciones de marque";
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(15, 65);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(161, 28);
			this.btnSelect.TabIndex = 1;
			this.btnSelect.Text = "Marca";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// cmbSays
			// 
			this.cmbSays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSays.FormattingEnabled = true;
			this.cmbSays.Location = new System.Drawing.Point(6, 19);
			this.cmbSays.Name = "cmbSays";
			this.cmbSays.Size = new System.Drawing.Size(181, 21);
			this.cmbSays.TabIndex = 0;
			// 
			// txtMarques
			// 
			this.txtMarques.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMarques.Location = new System.Drawing.Point(5, 210);
			this.txtMarques.Multiline = true;
			this.txtMarques.Name = "txtMarques";
			this.txtMarques.ReadOnly = true;
			this.txtMarques.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMarques.Size = new System.Drawing.Size(197, 175);
			this.txtMarques.TabIndex = 6;
			this.txtMarques.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10\r\n";
			// 
			// txtSays
			// 
			this.txtSays.Location = new System.Drawing.Point(8, 8);
			this.txtSays.Multiline = true;
			this.txtSays.Name = "txtSays";
			this.txtSays.ReadOnly = true;
			this.txtSays.Size = new System.Drawing.Size(194, 196);
			this.txtSays.TabIndex = 8;
			// 
			// UcSaySelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtSays);
			this.Controls.Add(this.grpSayOptions);
			this.Controls.Add(this.txtMarques);
			this.Name = "UcSaySelector";
			this.Size = new System.Drawing.Size(205, 528);
			this.grpSayOptions.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpSayOptions;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.ComboBox cmbSays;
		private System.Windows.Forms.TextBox txtMarques;
		private System.Windows.Forms.TextBox txtSays;
	}
}
