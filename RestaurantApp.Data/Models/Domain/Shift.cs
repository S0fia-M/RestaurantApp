using RestaurantApp.Data.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class Shift : TEntity<int>
    {
        [DisplayName("Доход")]
        public double Profit { get; set; }
        [DisplayName("Дата смены")]
        public DateTime ShiftDate { get; set; }
        public int EmployerId { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual List<ShiftsOrders> ShiftsOrders { get; set; }
    }
}
