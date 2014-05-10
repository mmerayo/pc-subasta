namespace Subasta.Lib.UserControls
	{
	partial class UcUserInteractionContainer
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
			this.ucSaySelector1 = new Subasta.Lib.UserControls.UcSaySelector();
			this.ucDeclarationSelector1 = new Subasta.Lib.UserControls.UcDeclarationSelector();
			this.ucTrumpSelection1 = new Subasta.Lib.UserControls.UcTrumpSelection();
			this.SuspendLayout();
			// 
			// ucSaySelector1
			// 
			this.ucSaySelector1.Location = new System.Drawing.Point(3, 3);
			this.ucSaySelector1.Name = "ucSaySelector1";
			this.ucSaySelector1.Size = new System.Drawing.Size(205, 447);
			this.ucSaySelector1.TabIndex = 0;
			// 
			// ucDeclarationSelector1
			// 
			this.ucDeclarationSelector1.Location = new System.Drawing.Point(3, 329);
			this.ucDeclarationSelector1.Name = "ucDeclarationSelector1";
			this.ucDeclarationSelector1.Size = new System.Drawing.Size(192, 88);
			this.ucDeclarationSelector1.TabIndex = 1;
			// 
			// ucTrumpSelection1
			// 
			this.ucTrumpSelection1.Location = new System.Drawing.Point(4, 329);
			this.ucTrumpSelection1.Name = "ucTrumpSelection1";
			this.ucTrumpSelection1.Size = new System.Drawing.Size(204, 121);
			this.ucTrumpSelection1.TabIndex = 2;
			// 
			// UcUserInteractionContainer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ucTrumpSelection1);
			this.Controls.Add(this.ucDeclarationSelector1);
			this.Controls.Add(this.ucSaySelector1);
			this.Name = "UcUserInteractionContainer";
			this.Size = new System.Drawing.Size(216, 473);
			this.ResumeLayout(false);

			}

		#endregion

		private UcSaySelector ucSaySelector1;
		private UcDeclarationSelector ucDeclarationSelector1;
		private UcTrumpSelection ucTrumpSelection1;
		}
	}
