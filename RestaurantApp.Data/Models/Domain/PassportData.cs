using RestaurantApp.Data.Abstraction;
using System;
using System.ComponentModel;

namespace RestaurantApp.Data.Models.Domain
{
    public class PassportData: TEntity<int>
    {
        [DisplayName("Серия")]
        public string PassportSeries { get; set; }
        [DisplayName("Номер")]
        public string PassportNumber { get; set; }
        [DisplayName("Пол")]
        public string Gender { get; set; }
        [DisplayName("Место рождения")]
        public string PlaceOfBirth { get; set; }
        [DisplayName("Кем выдан")]
        public string IssuedBy { get; set; }
        [DisplayName("Код подразделения")]
        public string DepartmentCode { get; set; }
        [DisplayName("Дата выдачи")]
        public DateTime IssuedDate { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime BirthDate { get; set; }
    }
}
