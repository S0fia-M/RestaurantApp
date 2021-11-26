using RestaurantApp.Data.Abstraction;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Position : TEntity<int>
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Оклад")]
        public double Salary { get; set; }
    }
}
