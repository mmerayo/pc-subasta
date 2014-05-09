using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Subasta.ApplicationServices.IO;
using Subasta.Client.Common.Game;

namespace Subasta.Lib
{
	public partial class FrmMain : Form
	{
		private readonly FrmChangeList _frmChangeList;
		private readonly IPathHelper _pathHelper;
		private readonly IGameSetHandler _gameSetHandler;
		private readonly FrmGameScreen _frmGameScreen;

		public FrmMain(IGameSetHandler gameSetHandler, FrmGameScreen frmGameScreen,
		               FrmChangeList frmChangeList,IPathHelper pathHelper)
		{
			InitializeComponent();

			_gameSetHandler = gameSetHandler;
			_frmGameScreen = frmGameScreen;
			_frmChangeList = frmChangeList;
			_pathHelper = pathHelper;

			_frmGameScreen.MdiParent = this;
			this.Size =new Size( _frmGameScreen.Width +10,_frmGameScreen.Height+33);

			SubscribeToGameSetEvents();
		}


		private void SubscribeToGameSetEvents()
		{
			_gameSetHandler.GameSetCompleted += _gameSet_GameSetCompleted;
		}


		private void _gameSet_GameSetCompleted(IGameSetHandler sender)
		{
			StartSet();
		}

		private void StartSet()
		{
			try
			{
				_gameSetHandler.Start();
			}
			catch (Exception ex)
			{
				//TODO: SEND ERROR
			}
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			ShowChangesDialog();
			_frmGameScreen.Show();

			StartSet();
		}

		private void ShowChangesDialog()
		{
			const string versionKeyName = "Version";
			string exePath = string.Format("{0}\\{1}.exe", _pathHelper.GetApplicationFolderPath(), Assembly.GetEntryAssembly().GetName().Name);
			Configuration exeConfig = ConfigurationManager.OpenExeConfiguration(exePath);
			string previousVersion=null;
			if(ConfigurationManager.AppSettings[versionKeyName]!=null)
				previousVersion = exeConfig.AppSettings.Settings[versionKeyName].Value;
			string currentVersion = GetType().Assembly.GetName().Version.ToString();

			if (previousVersion == null)
			{
				exeConfig.AppSettings.Settings.Add(versionKeyName, currentVersion);
				exeConfig.Save(ConfigurationSaveMode.Modified);
			}
			if (previousVersion != currentVersion)
			{
				exeConfig.AppSettings.Settings[versionKeyName].Value = currentVersion;
				exeConfig.Save(ConfigurationSaveMode.Modified);
				_frmChangeList.ShowDialog(this);
			}
		}
	}
}