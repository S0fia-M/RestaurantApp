using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.MVC.ViewModels.Accounts
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
