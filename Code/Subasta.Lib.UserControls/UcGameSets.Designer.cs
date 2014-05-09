namespace Subasta.Lib.UserControls
{
	partial class UcGameSets
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
		this.tabs = new System.Windows.Forms.TabControl();
		this.SuspendLayout();
		// 
		// tabs
		// 
		this.tabs.Location = new System.Drawing.Point(0, 0);
		this.tabs.Name = "tabs";
		this.tabs.SelectedIndex = 0;
		this.tabs.Size = new System.Drawing.Size(206, 395);
		this.tabs.TabIndex = 7;
		// 
		// UcGameSets
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.tabs);
		this.Name = "UcGameSets";
		this.Size = new System.Drawing.Size(206, 260);
		this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabs;

	}
}
