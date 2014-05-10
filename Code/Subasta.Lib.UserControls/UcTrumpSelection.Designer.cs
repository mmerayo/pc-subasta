namespace Subasta.Lib.UserControls
{
	partial class UcTrumpSelection
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
		this.label1 = new System.Windows.Forms.Label();
		this.pbOros = new System.Windows.Forms.PictureBox();
		this.pbCopas = new System.Windows.Forms.PictureBox();
		this.pbEspadas = new System.Windows.Forms.PictureBox();
		this.pbBastos = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)(this.pbOros)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.pbCopas)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.pbEspadas)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.pbBastos)).BeginInit();
		this.SuspendLayout();
		// 
		// label1
		// 
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.label1.Location = new System.Drawing.Point(14, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(144, 18);
		this.label1.TabIndex = 0;
		this.label1.Text = "Seleccione triunfo";
		// 
		// pbOros
		// 
		this.pbOros.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbOros.Location = new System.Drawing.Point(4, 26);
		this.pbOros.Name = "pbOros";
		this.pbOros.Size = new System.Drawing.Size(38, 49);
		this.pbOros.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pbOros.TabIndex = 1;
		this.pbOros.TabStop = false;
		this.pbOros.Click += new System.EventHandler(this.pb_Click);
		// 
		// pbCopas
		// 
		this.pbCopas.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbCopas.Location = new System.Drawing.Point(48, 26);
		this.pbCopas.Name = "pbCopas";
		this.pbCopas.Size = new System.Drawing.Size(38, 49);
		this.pbCopas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pbCopas.TabIndex = 2;
		this.pbCopas.TabStop = false;
		this.pbCopas.Click += new System.EventHandler(this.pb_Click);
		// 
		// pbEspadas
		// 
		this.pbEspadas.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbEspadas.Location = new System.Drawing.Point(92, 25);
		this.pbEspadas.Name = "pbEspadas";
		this.pbEspadas.Size = new System.Drawing.Size(38, 49);
		this.pbEspadas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pbEspadas.TabIndex = 3;
		this.pbEspadas.TabStop = false;
		this.pbEspadas.Click += new System.EventHandler(this.pb_Click);
		// 
		// pbBastos
		// 
		this.pbBastos.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbBastos.Location = new System.Drawing.Point(136, 26);
		this.pbBastos.Name = "pbBastos";
		this.pbBastos.Size = new System.Drawing.Size(38, 49);
		this.pbBastos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pbBastos.TabIndex = 4;
		this.pbBastos.TabStop = false;
		this.pbBastos.Click += new System.EventHandler(this.pb_Click);
		// 
		// UcTrumpSelection
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.pbBastos);
		this.Controls.Add(this.pbEspadas);
		this.Controls.Add(this.pbCopas);
		this.Controls.Add(this.pbOros);
		this.Controls.Add(this.label1);
		this.Name = "UcTrumpSelection";
		this.Size = new System.Drawing.Size(191, 89);
		((System.ComponentModel.ISupportInitialize)(this.pbOros)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.pbCopas)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.pbEspadas)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.pbBastos)).EndInit();
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbOros;
		private System.Windows.Forms.PictureBox pbCopas;
		private System.Windows.Forms.PictureBox pbEspadas;
		private System.Windows.Forms.PictureBox pbBastos;

	}
}
