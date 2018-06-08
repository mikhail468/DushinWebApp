using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DushinWebApp.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required(ErrorMessage = "Please enter the user name")]
        [MaxLength(20, ErrorMessage = "Name length greater than 20")]
        [MinLength(3, ErrorMessage = "Name less than 3")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [MaxLength(20, ErrorMessage = "Password length greater than 60")]
        [MinLength(3, ErrorMessage = "Password less than 3")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [MaxLength(20, ErrorMessage = "Password length greater than 60")]
        [MinLength(3, ErrorMessage = "Password less than 3")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
