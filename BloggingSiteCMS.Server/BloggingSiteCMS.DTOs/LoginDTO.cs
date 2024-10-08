using System;
using System.ComponentModel.DataAnnotations;

namespace BloggingSiteCMS.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}