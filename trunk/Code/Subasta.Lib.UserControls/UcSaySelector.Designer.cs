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
			this.tabs = new System.Windows.Forms.TabControl();
			this.Leyenda = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.grpSayOptions.SuspendLayout();
			this.tabs.SuspendLayout();
			this.Leyenda.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpSayOptions
			// 
			this.grpSayOptions.Controls.Add(this.btnSelect);
			this.grpSayOptions.Controls.Add(this.cmbSays);
			this.grpSayOptions.Location = new System.Drawing.Point(6, 330);
			this.grpSayOptions.Name = "grpSayOptions";
			this.grpSayOptions.Size = new System.Drawing.Size(195, 107);
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
			this.txtMarques.Location = new System.Drawing.Point(3, 3);
			this.txtMarques.Multiline = true;
			this.txtMarques.Name = "txtMarques";
			this.txtMarques.ReadOnly = true;
			this.txtMarques.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMarques.Size = new System.Drawing.Size(183, 286);
			this.txtMarques.TabIndex = 6;
			this.txtMarques.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10\r\n";
			// 
			// txtSays
			// 
			this.txtSays.BackColor = System.Drawing.SystemColors.Control;
			this.txtSays.Location = new System.Drawing.Point(5, 6);
			this.txtSays.Multiline = true;
			this.txtSays.Name = "txtSays";
			this.txtSays.ReadOnly = true;
			this.txtSays.Size = new System.Drawing.Size(186, 283);
			this.txtSays.TabIndex = 8;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tabPage2);
			this.tabs.Controls.Add(this.Leyenda);
			this.tabs.Location = new System.Drawing.Point(2, 3);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(200, 321);
			this.tabs.TabIndex = 9;
			// 
			// Leyenda
			// 
			this.Leyenda.Controls.Add(this.txtMarques);
			this.Leyenda.Location = new System.Drawing.Point(4, 22);
			this.Leyenda.Name = "Leyenda";
			this.Leyenda.Padding = new System.Windows.Forms.Padding(3);
			this.Leyenda.Size = new System.Drawing.Size(192, 295);
			this.Leyenda.TabIndex = 0;
			this.Leyenda.Text = "Leyenda";
			this.Leyenda.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtSays);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(192, 295);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Estado";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// UcSaySelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.grpSayOptions);
			this.Name = "UcSaySelector";
			this.Size = new System.Drawing.Size(205, 447);
			this.grpSayOptions.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.Leyenda.ResumeLayout(false);
			this.Leyenda.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpSayOptions;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.ComboBox cmbSays;
		private System.Windows.Forms.TextBox txtMarques;
		private System.Windows.Forms.TextBox txtSays;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage Leyenda;
		private System.Windows.Forms.TabPage tabPage2;
	}
}
