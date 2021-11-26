using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class MenuItem : TEntity<int>
    {
        [DisplayName("Наименование блюда")]
        public string PositionName { get; set; }
        [DisplayName("Вес")]
        public double Weight { get; set; }
        [DisplayName("Цена")]
        public double Price { get; set; }
        public virtual List<MenuItemsCompositions> MenuItemsCompositions { get; set; }
        public virtual List<OrdersMenuItems> OrdersMenuItems { get; set; }
    }
}
