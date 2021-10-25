using FoodOrderingService.Entities;
using FoodOrderingService.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FoodOrderingService.Controllers
{
     class PostOrderController : Controller
     {
          private static readonly Lazy<Controller> controllerInstance = new(() => new PostOrderController());

          public static Controller Instance { get { return controllerInstance.Value; } }

          private PostOrderController() : base("POST", "/order")
          {
          }

          public override void HandleRequest(HttpListenerContext httpListenerContext)
          {
               StreamReader stream = new(httpListenerContext.Request.InputStream);
               string streamClientOrder = stream.ReadToEnd();

               var clientOrder = JsonConvert.DeserializeObject<ClientOrder>(streamClientOrder);
               List<OrderEstimation> ordersEstimations = new();

               foreach (var order in clientOrder.Orders)
               {
                    SendRequestService.SendPostRequest($"{Constants.DINING_HALL_ADDRESS}/v2/order", JsonConvert.SerializeObject(new
                    {
                         order.Items,
                         order.Priority,
                         order.MaxWait,
                         order.CreatedTime
                    }), out string response);
                    var orderEstimation = JsonConvert.DeserializeObject<OrderEstimation>(response);
                    orderEstimation.RestaurantAddress = FoodOrderingService.Instance.Restaurants.Single(restaurant => order.RestaurantId == restaurant.Id).Address;
                    ordersEstimations.Add(orderEstimation);
               }

               var orderEstimationResponse = new
               {
                    OrderId = Guid.NewGuid().GetHashCode(),
                    Orders = ordersEstimations
               };

               httpListenerContext.Response.StatusCode = 200;
               httpListenerContext.Response.ContentType = "text/plain";
               byte[] responseBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderEstimationResponse));
               httpListenerContext.Response.ContentLength64 = responseBuffer.Length;
               Stream output = httpListenerContext.Response.OutputStream;
               output.Write(responseBuffer, 0, responseBuffer.Length);
               output.Close();
          }
     }
}
