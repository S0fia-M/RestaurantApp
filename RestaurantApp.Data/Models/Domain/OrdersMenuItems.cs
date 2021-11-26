namespace RestaurantApp.Data.Models.Domain
{
    public class OrdersMenuItems
    {
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }
    }
}
