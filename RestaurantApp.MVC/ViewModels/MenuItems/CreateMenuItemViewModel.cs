using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.MVC.ViewModels.MenuItems
{
    public class CreateMenuItemViewModel
    {
        public int Id { get; set; }
        [DisplayName("Наименование блюда")]
        public string PositionName { get; set; }
        [DisplayName("Вес")]
        public double Weight { get; set; }
        [DisplayName("Цена")]
        public double Price { get; set; }

        public List<SelectListItem> Ingridients { get; set; }
    }
}
