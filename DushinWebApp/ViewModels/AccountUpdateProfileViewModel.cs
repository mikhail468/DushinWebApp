using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DushinWebApp.ViewModels
{
    public class AccountUpdateProfileViewModel
    {
        [MaxLength(60, ErrorMessage = "Name length greater than 60")]
        public string FirstName { get; set; }
        [MaxLength(60, ErrorMessage = "Name length greater than 60")]
        public string LastName { get; set; }
        [MaxLength(60, ErrorMessage = "Email length greater than 60")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "Mobile length greater than 20")]
        public string Mobile { get; set; }
        public string Photo { get; set; }
    }
}
