using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
	internal class ServiceEventsPublisher : IEventPublisher
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof (ServiceEventsPublisher));
		private readonly IPathHelper _pathHelper;

        public ServiceEventsPublisher(IPathHelper pathHelper)
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
            
		    using (var client = new HttpClient())
		    {
                var requestContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileName));
                fileContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("text/plain");

                requestContent.Add(fileContent, "text", Path.GetFileName(fileName));
		        try
		        {
                    var result= client.PostAsync("http://localhost/subastabackend/api/analytics/post", requestContent).Result;
		        }
		        catch (Exception ex)
		        {
		            Logger.Warn(ex);
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