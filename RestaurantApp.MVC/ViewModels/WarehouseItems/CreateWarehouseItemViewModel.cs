using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.MVC.ViewModels.WarehouseItems
{
    public class CreateWarehouseItemViewModel
    {
        public int Id { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Поставщик")]
        public string Provider { get; set; }
        [DisplayName("Расход")]
        public double Expenses { get; set; }
        public List<SelectListItem> Products { get; set; }
    }
}
