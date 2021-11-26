using RestaurantApp.Data.Abstraction;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Employer : TEntity<int> 
    {
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }
        public int PassportDataId { get; set; }
        public virtual PassportData PassportData { get; set; }
        public virtual List<Shift> ShiftOrders { get; set; }
    }
}
