using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;

namespace RestaurantApp.Data.Models.Domain
{
    public class Order : TEntity<int>
    {
        public virtual List<OrdersMenuItems> OrdersMenuItems { get; set; }
        public virtual List<ShiftsOrders> ShiftsOrders { get; set; }
    }
}
