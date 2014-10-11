using System;
using System.Net;

namespace Subasta.Domain.Events
{
	[Serializable]
	public abstract class ApplicationEvent : IAppEvent
	{
		private const string Unknown = "Unknown";
		private static string _hostName;

		private static string _userName;

		private static string _ipAddress;


		private readonly DateTime _created = DateTime.UtcNow;

		#region IAppEvent Members

		public string HostName
		{
			get { return _hostName ?? (_hostName = GetSystemProperty(() => Environment.MachineName)); }
			set { }
		}

		public string UserName
		{
			get { return _userName ?? (_userName = GetSystemProperty(() => Environment.UserName)); }
			set { }
		}

		public string IP
		{
			get { return _ipAddress ?? (_ipAddress = FindLanAddress()); }
			set { }
		}

		public DateTime DateTimeUtc
		{
			get { return _created; }
			set { }
		}

		public string EventName
		{
			get { return GetType().Name; }
			set { }
		}

		#endregion

		private static string GetSystemProperty(Func<string> action)
		{
			string result;
			try
			{
				result = action();
			}
			catch
			{
				result = Unknown;
			}

			return result;
		}


		private static string FindLanAddress()
		{
			var urls = new[] {"http://ipinfo.io/ip", "http://icanhazip.com", "http://bot.whatismyipaddress.com"};

			using (var webClient = new WebClient())
				foreach (string url in urls)
				{
					string result = GetSystemProperty(() => webClient.DownloadString(url));
					if (result != Unknown) return result.TrimEnd('\r', '\n'); ;
				}

			return Unknown;
		}
	}
}