using System;

namespace RestaurantApp.MVC.ViewModels.PassportData
{
    public class CreatePassportDataViewModel
    {
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IssuedBy { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
