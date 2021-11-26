namespace RestaurantApp.Data.Models.Domain
{
    public class ShiftsOrders
    {
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
