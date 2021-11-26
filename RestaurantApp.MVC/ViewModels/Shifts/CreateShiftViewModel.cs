using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantApp.MVC.ViewModels.Shifts
{
    public class CreateShiftViewModel
    {
        public int Id { get; set; }
        [DisplayName("Доход")]
        public double Profit { get; set; }
        [DisplayName("Дата смены")]
        public DateTime ShiftDate { get; set; }
        public int EmployerId { get; set; }

        public List<SelectListItem> Orders { get; set; }
    }
}
