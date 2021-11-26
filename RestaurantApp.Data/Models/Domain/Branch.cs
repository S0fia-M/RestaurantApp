using RestaurantApp.Data.Abstraction;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Branch : TEntity<int>
    {
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
        public int EmployerId { get; set; }
        public virtual Employer Employer { get; set; }
        public int WarehouseItemId { get; set; }
        public virtual WarehouseItem WarehouseItem { get; set; }
    }
}
