using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class WarehouseItem : TEntity<int>
    {
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Поставщик")]
        public string Provider { get; set; }
        [DisplayName("Расход")]
        public double Expenses { get; set; }
        public virtual List<WarehouseItemsProducts> WarehouseItemsProducts { get; set; }
    }
}
