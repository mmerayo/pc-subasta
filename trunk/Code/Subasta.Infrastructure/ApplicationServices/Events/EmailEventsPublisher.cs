using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Subasta.ApplicationServices.Events;
using Subasta.ApplicationServices.IO;
using Subasta.Domain.Events;
using log4net;

namespace Subasta.Infrastructure.ApplicationServices.Events
{
	internal class EmailEventsPublisher : IEventPublisher
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (EmailEventsPublisher));
		private readonly IPathHelper _pathHelper;

		public EmailEventsPublisher(IPathHelper pathHelper)
		{
			_pathHelper = pathHelper;
		}

		#region IEventPublisher Members

		public void Publish<TEventData>(TEventData data) where TEventData : IAppEvent
		{
			if (data == null) throw new ArgumentNullException("data");

			Task.Factory.StartNew(() => DoPublish(data));
		}

		public void PublishSync<TEventData>(TEventData data) where TEventData : IAppEvent
		{
			if (data == null) throw new ArgumentNullException("data");

			DoPublish(data);
		}

		private void DoPublish<TEventData>(TEventData data) where TEventData : IAppEvent
		{
			string fileName =
				_pathHelper.GetApplicationFolderPathForFile(string.Format("{0}_{1}.subastaEvent", data.GetType().Name,
				                                                          Guid.NewGuid().ToString().Replace('-', '_')));

			try
			{
				Serialize(data, fileName);
				SendEvent(fileName);
			}
			catch (Exception ex)
			{
				Logger.Warn("Could not publish " + fileName, ex);
			}
			finally
			{
				try
				{
					File.Delete(fileName);
				}
				catch
				{
					//SWALLOW
				}
			}
		}

		#endregion

		protected virtual void SendEvent(string fileName)
		{
			const string smtpAddress = "smtp.live.com";
			const int portNumber = 587;

			const string emailFrom = "pc-subasta@outlook.com";
			const string password = "$$Password!!1";

			const string emailTo = "miguelmerayo@hotmail.com";
			string subject = string.Format("Subasta event {0}", Path.GetFileName(fileName));
			string body = File.ReadAllText(fileName);

			using (var mail = new MailMessage())
			{
				mail.From = new MailAddress(emailFrom);
				mail.To.Add(emailTo);
				mail.Subject = subject;
				mail.Body = body;
				mail.IsBodyHtml = false;

				mail.Attachments.Add(new Attachment(fileName));

				using (var smtp = new SmtpClient(smtpAddress, portNumber))
				{
					smtp.Credentials = new NetworkCredential(emailFrom, password);
					smtp.EnableSsl = true;
					smtp.Send(mail);
				}
			}
		}

		private static void Serialize<TEventData>(TEventData data, string fileName) where TEventData : IAppEvent
		{
			var xmlDocument = new XmlDocument();
			var serializer = new XmlSerializer(data.GetType());
			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, data);
				stream.Position = 0;
				xmlDocument.Load(stream);
				xmlDocument.Save(fileName);
				stream.Close();
			}
		}
	}
}