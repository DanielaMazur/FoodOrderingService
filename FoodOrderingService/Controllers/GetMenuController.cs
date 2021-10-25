using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace FoodOrderingService.Controllers
{
     class GetMenuController : Controller
     {
          private static readonly Lazy<Controller> controllerInstance = new(() => new GetMenuController());

          public static Controller Instance { get { return controllerInstance.Value; } }

          private GetMenuController() : base("GET", "/menu")
          {
          }
          public override void HandleRequest(HttpListenerContext httpListenerContext)
          {
               httpListenerContext.Response.StatusCode = 200;
               httpListenerContext.Response.ContentType = "Application/json";

               byte[] responseBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { 
                    Restaurants = FoodOrderingService.Instance.Restaurants.Count,
                    RestaurantsData = FoodOrderingService.Instance.Restaurants
               }));
               httpListenerContext.Response.ContentLength64 = responseBuffer.Length;
               Stream output = httpListenerContext.Response.OutputStream;
               output.Write(responseBuffer, 0, responseBuffer.Length);
               output.Close();
          }
     }
}
