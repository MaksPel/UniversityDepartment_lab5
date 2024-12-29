using System.ComponentModel.DataAnnotations;

namespace UniversityDepartment_lab5.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }
}
