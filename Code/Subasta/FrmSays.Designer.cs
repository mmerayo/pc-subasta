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
			this.cmbSays = new System.Windows.Forms.ComboBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.grpSayOptions = new System.Windows.Forms.GroupBox();
			this.grpTrumpOptions = new System.Windows.Forms.GroupBox();
			this.btnSelectTrump = new System.Windows.Forms.Button();
			this.cmbSuits = new System.Windows.Forms.ComboBox();
			this.grpSayOptions.SuspendLayout();
			this.grpTrumpOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmbSays
			// 
			this.cmbSays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSays.FormattingEnabled = true;
			this.cmbSays.Location = new System.Drawing.Point(44, 19);
			this.cmbSays.Name = "cmbSays";
			this.cmbSays.Size = new System.Drawing.Size(287, 21);
			this.cmbSays.TabIndex = 0;
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(351, 21);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(161, 28);
			this.btnSelect.TabIndex = 1;
			this.btnSelect.Text = "Marca";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// grpSayOptions
			// 
			this.grpSayOptions.Controls.Add(this.btnSelect);
			this.grpSayOptions.Controls.Add(this.cmbSays);
			this.grpSayOptions.Location = new System.Drawing.Point(12, 0);
			this.grpSayOptions.Name = "grpSayOptions";
			this.grpSayOptions.Size = new System.Drawing.Size(584, 76);
			this.grpSayOptions.TabIndex = 2;
			this.grpSayOptions.TabStop = false;
			this.grpSayOptions.Text = "Opciones de marque";
			// 
			// grpTrumpOptions
			// 
			this.grpTrumpOptions.Controls.Add(this.btnSelectTrump);
			this.grpTrumpOptions.Controls.Add(this.cmbSuits);
			this.grpTrumpOptions.Location = new System.Drawing.Point(12, 92);
			this.grpTrumpOptions.Name = "grpTrumpOptions";
			this.grpTrumpOptions.Size = new System.Drawing.Size(584, 76);
			this.grpTrumpOptions.TabIndex = 3;
			this.grpTrumpOptions.TabStop = false;
			this.grpTrumpOptions.Text = "Seleccion de triunfo";
			// 
			// btnSelectTrump
			// 
			this.btnSelectTrump.Location = new System.Drawing.Point(351, 21);
			this.btnSelectTrump.Name = "btnSelectTrump";
			this.btnSelectTrump.Size = new System.Drawing.Size(161, 28);
			this.btnSelectTrump.TabIndex = 1;
			this.btnSelectTrump.Text = "Seleccionar";
			this.btnSelectTrump.UseVisualStyleBackColor = true;
			this.btnSelectTrump.Click += new System.EventHandler(this.btnSelectTrump_Click);
			// 
			// cmbSuits
			// 
			this.cmbSuits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSuits.FormattingEnabled = true;
			this.cmbSuits.Location = new System.Drawing.Point(44, 19);
			this.cmbSuits.Name = "cmbSuits";
			this.cmbSuits.Size = new System.Drawing.Size(287, 21);
			this.cmbSuits.TabIndex = 0;
			// 
			// FrmSays
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(613, 81);
			this.ControlBox = false;
			this.Controls.Add(this.grpSayOptions);
			this.Controls.Add(this.grpTrumpOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmSays";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Marcar";
			this.grpSayOptions.ResumeLayout(false);
			this.grpTrumpOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbSays;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.GroupBox grpSayOptions;
		private System.Windows.Forms.GroupBox grpTrumpOptions;
		private System.Windows.Forms.Button btnSelectTrump;
		private System.Windows.Forms.ComboBox cmbSuits;
	}
}