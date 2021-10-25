﻿using System.Collections.Generic;

namespace FoodOrderingService.Entities
{
     class ClientOrder
     {
          public int ClientId { get; set; }
          public List<OrderItem> Orders = new();
          public ClientOrder(int clientId, List<OrderItem> orders)
          {
               ClientId = clientId;
               Orders = orders;
          }
     }
}
