namespace Analyzer
{
	partial class FrmMain
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
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openGameFile = new System.Windows.Forms.OpenFileDialog();
		this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.menuStrip1.SuspendLayout();
		this.SuspendLayout();
		// 
		// menuStrip1
		// 
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.statusToolStripMenuItem});
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(822, 24);
		this.menuStrip1.TabIndex = 1;
		this.menuStrip1.Text = "menuStrip1";
		// 
		// fileToolStripMenuItem
		// 
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem});
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.fileToolStripMenuItem.Text = "File";
		// 
		// NewToolStripMenuItem
		// 
		this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
		this.NewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.NewToolStripMenuItem.Text = "Load";
		this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
		// 
		// statusToolStripMenuItem
		// 
		this.statusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.showToolStripMenuItem});
		this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
		this.statusToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
		this.statusToolStripMenuItem.Text = "Operations";
		// 
		// startToolStripMenuItem
		// 
		this.startToolStripMenuItem.Name = "startToolStripMenuItem";
		this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.startToolStripMenuItem.Text = "Start";
		this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
		// 
		// openGameFile
		// 
		this.openGameFile.FileName = "Start.Data";
		this.openGameFile.Filter = "new game files|*.data";
		// 
		// showToolStripMenuItem
		// 
		this.showToolStripMenuItem.Name = "showToolStripMenuItem";
		this.showToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
		this.showToolStripMenuItem.Text = "Show";
		this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
		// 
		// FrmMain
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(822, 703);
		this.Controls.Add(this.menuStrip1);
		this.IsMdiContainer = true;
		this.MainMenuStrip = this.menuStrip1;
		this.Name = "FrmMain";
		this.Text = "Analyzer";
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openGameFile;
		private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
	}
}

