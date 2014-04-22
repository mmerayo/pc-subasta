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
			this.SuspendLayout();
			// 
			// cmbSays
			// 
			this.cmbSays.FormattingEnabled = true;
			this.cmbSays.Location = new System.Drawing.Point(13, 13);
			this.cmbSays.Name = "cmbSays";
			this.cmbSays.Size = new System.Drawing.Size(287, 21);
			this.cmbSays.TabIndex = 0;
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(320, 15);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(161, 28);
			this.btnSelect.TabIndex = 1;
			this.btnSelect.Text = "Marca";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// FrmSays
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(621, 83);
			this.ControlBox = false;
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.cmbSays);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmSays";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Marcar";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbSays;
		private System.Windows.Forms.Button btnSelect;
	}
}