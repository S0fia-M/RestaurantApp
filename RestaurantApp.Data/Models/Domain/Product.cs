using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Product : TEntity<int>
    {
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Склад")]
        public virtual List<WarehouseItemsProducts> WarehouseItemsProducts { get; set; }
    }
}
