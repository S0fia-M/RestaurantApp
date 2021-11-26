using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantApp.Data.Models.Domain;
using System.Collections.Generic;

namespace RestaurantApp.MVC.ViewModels.MenuItems
{
    public class MenuItemListViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }

        public string Name { get; set; }

        public List<string> Ingridients { get; set; }
    }
}
