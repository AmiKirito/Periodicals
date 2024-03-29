﻿using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents registering user ViewModel for business logic and presentation layers
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should contain at least 5 symbols")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Please enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}