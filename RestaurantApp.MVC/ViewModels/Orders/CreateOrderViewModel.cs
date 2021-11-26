using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.MVC.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        public int OrderId { get; set; }
        public List<SelectListItem> MenuItems { get; set; }
    }
}
