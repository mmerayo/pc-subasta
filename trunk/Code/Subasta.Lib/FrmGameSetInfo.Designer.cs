namespace Subasta.Lib
{
	partial class FrmGameSetInfo
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
			this.txtGameSetStatus = new System.Windows.Forms.TextBox();
			this.grpSayOptions = new System.Windows.Forms.GroupBox();
			this.grpDeclarations = new System.Windows.Forms.GroupBox();
			this.btnDeclarations = new System.Windows.Forms.Button();
			this.cmbDeclarations = new System.Windows.Forms.ComboBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.cmbSays = new System.Windows.Forms.ComboBox();
			this.grpTrumpOptions = new System.Windows.Forms.GroupBox();
			this.btnSelectTrump = new System.Windows.Forms.Button();
			this.cmbSuits = new System.Windows.Forms.ComboBox();
			this.grpSayOptions.SuspendLayout();
			this.grpDeclarations.SuspendLayout();
			this.grpTrumpOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtGameSetStatus
			// 
			this.txtGameSetStatus.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGameSetStatus.Location = new System.Drawing.Point(13, 13);
			this.txtGameSetStatus.Multiline = true;
			this.txtGameSetStatus.Name = "txtGameSetStatus";
			this.txtGameSetStatus.ReadOnly = true;
			this.txtGameSetStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGameSetStatus.Size = new System.Drawing.Size(195, 381);
			this.txtGameSetStatus.TabIndex = 0;
			// 
			// grpSayOptions
			// 
			this.grpSayOptions.Controls.Add(this.grpDeclarations);
			this.grpSayOptions.Controls.Add(this.btnSelect);
			this.grpSayOptions.Controls.Add(this.cmbSays);
			this.grpSayOptions.Location = new System.Drawing.Point(13, 400);
			this.grpSayOptions.Name = "grpSayOptions";
			this.grpSayOptions.Size = new System.Drawing.Size(195, 122);
			this.grpSayOptions.TabIndex = 3;
			this.grpSayOptions.TabStop = false;
			this.grpSayOptions.Text = "Opciones de marque";
			this.grpSayOptions.Visible = false;
			// 
			// grpDeclarations
			// 
			this.grpDeclarations.Controls.Add(this.btnDeclarations);
			this.grpDeclarations.Controls.Add(this.cmbDeclarations);
			this.grpDeclarations.Location = new System.Drawing.Point(3, 0);
			this.grpDeclarations.Name = "grpDeclarations";
			this.grpDeclarations.Size = new System.Drawing.Size(192, 88);
			this.grpDeclarations.TabIndex = 5;
			this.grpDeclarations.TabStop = false;
			this.grpDeclarations.Text = "Cantar";
			// 
			// btnDeclarations
			// 
			this.btnDeclarations.Location = new System.Drawing.Point(12, 46);
			this.btnDeclarations.Name = "btnDeclarations";
			this.btnDeclarations.Size = new System.Drawing.Size(161, 28);
			this.btnDeclarations.TabIndex = 1;
			this.btnDeclarations.Text = "Canta";
			this.btnDeclarations.UseVisualStyleBackColor = true;
			this.btnDeclarations.Click += new System.EventHandler(this.btnDeclarations_Click);
			// 
			// cmbDeclarations
			// 
			this.cmbDeclarations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDeclarations.FormattingEnabled = true;
			this.cmbDeclarations.Location = new System.Drawing.Point(6, 19);
			this.cmbDeclarations.Name = "cmbDeclarations";
			this.cmbDeclarations.Size = new System.Drawing.Size(167, 21);
			this.cmbDeclarations.TabIndex = 0;
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
			// grpTrumpOptions
			// 
			this.grpTrumpOptions.Controls.Add(this.btnSelectTrump);
			this.grpTrumpOptions.Controls.Add(this.cmbSuits);
			this.grpTrumpOptions.Location = new System.Drawing.Point(12, 401);
			this.grpTrumpOptions.Name = "grpTrumpOptions";
			this.grpTrumpOptions.Size = new System.Drawing.Size(196, 81);
			this.grpTrumpOptions.TabIndex = 4;
			this.grpTrumpOptions.TabStop = false;
			this.grpTrumpOptions.Text = "Seleccion de triunfo";
			this.grpTrumpOptions.Visible = false;
			// 
			// btnSelectTrump
			// 
			this.btnSelectTrump.Location = new System.Drawing.Point(6, 46);
			this.btnSelectTrump.Name = "btnSelectTrump";
			this.btnSelectTrump.Size = new System.Drawing.Size(115, 28);
			this.btnSelectTrump.TabIndex = 1;
			this.btnSelectTrump.Text = "Seleccionar";
			this.btnSelectTrump.UseVisualStyleBackColor = true;
			this.btnSelectTrump.Click += new System.EventHandler(this.btnSelectTrump_Click);
			// 
			// cmbSuits
			// 
			this.cmbSuits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSuits.FormattingEnabled = true;
			this.cmbSuits.Location = new System.Drawing.Point(6, 19);
			this.cmbSuits.Name = "cmbSuits";
			this.cmbSuits.Size = new System.Drawing.Size(115, 21);
			this.cmbSuits.TabIndex = 0;
			// 
			// FrmGameSetInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(214, 545);
			this.ControlBox = false;
			this.Controls.Add(this.grpTrumpOptions);
			this.Controls.Add(this.grpSayOptions);
			this.Controls.Add(this.txtGameSetStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FrmGameSetInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Partida";
			this.grpSayOptions.ResumeLayout(false);
			this.grpDeclarations.ResumeLayout(false);
			this.grpTrumpOptions.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtGameSetStatus;
		private System.Windows.Forms.GroupBox grpSayOptions;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.ComboBox cmbSays;
		private System.Windows.Forms.GroupBox grpTrumpOptions;
		private System.Windows.Forms.Button btnSelectTrump;
		private System.Windows.Forms.ComboBox cmbSuits;
		private System.Windows.Forms.GroupBox grpDeclarations;
		private System.Windows.Forms.Button btnDeclarations;
		private System.Windows.Forms.ComboBox cmbDeclarations;
	}
}