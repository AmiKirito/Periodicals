using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents password change ViewModel for business logic and presentation layers
    /// </summary>
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Old password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match.")]
        public string NewPasswordConfirmation { get; set; }
    }
}