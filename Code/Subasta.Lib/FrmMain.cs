using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Subasta.ApplicationServices.IO;
using Subasta.Client.Common.Extensions;
using Subasta.Client.Common.Game;
using Subasta.Domain.Game;
using Subasta.Lib.Properties;

namespace Subasta.Lib
{
	public partial class FrmMain : Form
	{
		private readonly FrmChangeList _frmChangeList;
		private readonly IPathHelper _pathHelper;
		private readonly FrmGame _frmGame;
		private readonly FrmGameInfo _frmGameInfo;
		private readonly FrmGameSetInfo _frmGameSetInfo;
		private readonly IGameSetHandler _gameSetHandler;

		public FrmMain(IGameSetHandler gameSetHandler, FrmGame frmGame,
		               FrmGameInfo frmGameInfo, FrmGameSetInfo frmGameSetInfo, FrmChangeList frmChangeList,IPathHelper pathHelper)
		{
			InitializeComponent();

			_gameSetHandler = gameSetHandler;
			_frmGame = frmGame;
			_frmGameInfo = frmGameInfo;
			_frmGameSetInfo = frmGameSetInfo;
			_frmChangeList = frmChangeList;
			_pathHelper = pathHelper;

			_frmGame.MdiParent = _frmGameInfo.MdiParent = _frmGameSetInfo.MdiParent = this;

			//InitializePositionManagement();

			SubscribeToGameSetEvents();
		}

		private void UpdateFormsLocationsAndSizes()
		{
			_frmGameSetInfo.Left = _frmGameSetInfo.Top = 0;

			_frmGame.PerformSafely(
				(x) => x.Location = new Point(_frmGameSetInfo.Location.X + _frmGameSetInfo.Size.Width, _frmGameSetInfo.Location.Y));


			_frmGameInfo.PerformSafely((x) => x.Location = new Point(_frmGame.Left + _frmGame.Width, _frmGame.Top));
			_frmGameInfo.PerformSafely(x => x.Height = _frmGame.Height);


			_frmGameSetInfo.PerformSafely(x => x.Height = _frmGame.Height);

			this.PerformSafely(x =>
			                   {
			                   	Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
			                   	int titleHeight = screenRectangle.Top - Top;
			                   	const int borderSize = 10;
			                   	x.Width = _frmGameInfo.Width + _frmGameSetInfo.Width + _frmGame.Width + borderSize;
			                   	x.Height = _frmGame.Height + titleHeight + borderSize;
			                   }
				);
		}

		private void SubscribeToGameSetEvents()
		{
			_gameSetHandler.GameStarted += _gameSet_GameStarted;
			_gameSetHandler.GameCompleted += _gameSet_GameCompleted;

			_gameSetHandler.GameSaysCompleted += _gameSet_GameSaysCompleted;
			_gameSetHandler.GameSaysStarted += _gameSet_GameSaysStarted;

			_gameSetHandler.GameSetCompleted += _gameSet_GameSetCompleted;
			_gameSetHandler.GameSetStarted += _gameSet_GameSetStarted;
		}


		private void _gameSet_GameStarted(IExplorationStatus status)
		{
		}

		private void _gameSet_GameSetStarted(IGameSetHandler sender)
		{
			_frmGameSetInfo.PerformSafely((x) => x.Show());
			_frmGame.PerformSafely((x) => x.Show());
			_frmGameInfo.PerformSafely((x) => x.Show());
			UpdateFormsLocationsAndSizes();

			_frmGameSetInfo.PerformSafely(x => x.BringToFront());
			_frmGame.PerformSafely(x => x.BringToFront());
			_frmGameInfo.PerformSafely(x => x.BringToFront());
		}

		private void _gameSet_GameSetCompleted(IGameSetHandler sender)
		{
			StartSet();
		}

		private void _gameSet_GameSaysStarted(ISaysStatus status)
		{
		}

		private void _gameSet_GameSaysCompleted(ISaysStatus status)
		{
		}

		private void _gameSet_GameCompleted(IExplorationStatus status)
		{
		}

		private void InitializePositionManagement()
		{
			FormClosing += FrmMain_FormClosing;
			LocationChanged += FrmMain_LocationChanged;
			Resize += FrmMain_Resize;
			StartPosition = FormStartPosition.Manual;
			Location = Settings.Default.FrmMainLocation;
			WindowState = Settings.Default.FrmMainState;
			if (WindowState == FormWindowState.Normal)
				Size = Settings.Default.FrmMainSize;
		}

		private void FrmMain_Resize(object sender, EventArgs e)
		{
			Settings.Default.FrmMainState = WindowState;
			if (WindowState == FormWindowState.Normal)
				Settings.Default.FrmMainSize = Size;
		}

		private void FrmMain_LocationChanged(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
				Settings.Default.FrmMainLocation = Location;
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.Save();
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