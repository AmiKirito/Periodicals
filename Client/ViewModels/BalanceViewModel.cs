using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents balance ViewModel for business logic and presentation layers
    /// </summary>
    public class BalanceViewModel
    {
        public int Balance { get; set; }
        [Required(ErrorMessage = "Sum field is required")]
        [Display(Name = "Sum: ")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "Please enter valid sum")]
        public int AddSum { get; set; }

    }
}