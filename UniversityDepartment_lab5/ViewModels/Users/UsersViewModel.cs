using System.ComponentModel.DataAnnotations;

namespace UniversityDepartment_lab5.ViewModels.Users
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string Email { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
