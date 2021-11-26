using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Data.Models.Domain
{
    public class WarehouseItemsProducts
    {
        public int WarehouseItemId { get; set; }

        public virtual WarehouseItem WarehouseItem { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
