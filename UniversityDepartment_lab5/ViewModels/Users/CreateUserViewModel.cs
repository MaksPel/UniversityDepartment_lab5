using System.ComponentModel.DataAnnotations;

namespace UniversityDepartment_lab5.ViewModels.Users
{
    public class CreateUserViewModel
    {
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Display(Name = "Роль")]
        public string UserRole { get; set; }
        public CreateUserViewModel()
        {
            UserRole = "user";
        }
    }
}
