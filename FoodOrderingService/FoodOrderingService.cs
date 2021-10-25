using FoodOrderingService.Entities;
using System;
using System.Collections.Generic;

namespace FoodOrderingService
{
     class FoodOrderingService
     {
          private static readonly Lazy<FoodOrderingService> foodOrderingInstance = new(() => new FoodOrderingService());
          public static FoodOrderingService Instance { get { return foodOrderingInstance.Value; } }

          public List<Restaurant> Restaurants = new();

          private FoodOrderingService()
          {
          }
     }
}
