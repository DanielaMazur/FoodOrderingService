using System.IO;
using System.Net;
using System.Text;
 
namespace FoodOrderingService.Services
{
     class SendRequestService
     {
          public static void SendPostRequest(string url, string data, out string response)
          {
               WebRequest webRequest = WebRequest.Create(url);
               webRequest.Method = "POST";

               byte[] encodedData = Encoding.UTF8.GetBytes(data);
               webRequest.ContentType = "application/json";
               webRequest.ContentLength = encodedData.Length;

               Stream dataStream = webRequest.GetRequestStream();
               dataStream.Write(encodedData, 0, encodedData.Length);
               dataStream.Close();

               WebResponse webResponse = webRequest.GetResponse();
               using (dataStream = webResponse.GetResponseStream())
               {
                    StreamReader reader = new(dataStream);
                    response = reader.ReadToEnd();
               }
               webResponse.Close();
          }

          public static void SendGetRequest(string url, out string response)
          {
               WebRequest webRequest = WebRequest.Create(url);
               webRequest.Method = "GET";

               WebResponse webResponse = webRequest.GetResponse();
               using (Stream dataStream = webResponse.GetResponseStream())
               {
                    StreamReader reader = new(dataStream);
                    response = reader.ReadToEnd();

               }
               webResponse.Close();
          }
     }
}
