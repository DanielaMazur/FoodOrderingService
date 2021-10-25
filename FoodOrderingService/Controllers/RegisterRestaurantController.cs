using FoodOrderingService.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace FoodOrderingService.Controllers
{
     class RegisterRestaurantController : Controller
     {
          private static readonly Lazy<Controller> controllerInstance = new(() => new RegisterRestaurantController());

          public static Controller Instance { get { return controllerInstance.Value; } }

          private RegisterRestaurantController() : base("POST", "/register")
          {
          }
          public override void HandleRequest(HttpListenerContext httpListenerContext)
          {
               StreamReader stream = new(httpListenerContext.Request.InputStream);
               string streamRestaurant = stream.ReadToEnd();

               var restaurant = JsonConvert.DeserializeObject<Restaurant>(streamRestaurant);
               FoodOrderingService.Instance.Restaurants.Add(restaurant);

               Console.WriteLine($"Restaurant {restaurant.Name} successfully registered!");

               httpListenerContext.Response.StatusCode = 200;
               httpListenerContext.Response.ContentType = "text/plain";
               byte[] responseBuffer = Encoding.UTF8.GetBytes($"Restaurant {restaurant.Name} successfully registered!");
               httpListenerContext.Response.ContentLength64 = responseBuffer.Length;
               Stream output = httpListenerContext.Response.OutputStream;
               output.Write(responseBuffer, 0, responseBuffer.Length);
               output.Close();
          }
     }
}
