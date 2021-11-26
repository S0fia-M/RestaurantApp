using RestaurantApp.Data.Abstraction;
using System;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class HumanResourcesDepartments : TEntity<int>
    {
        [DisplayName("Дата приема на работу")]
        public DateTime EmploymentDate { get; set; }
        public int EmployerId { get; set; }
        public virtual Employer Employer { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
