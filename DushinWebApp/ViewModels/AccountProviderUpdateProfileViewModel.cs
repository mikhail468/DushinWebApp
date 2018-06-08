using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DushinWebApp.ViewModels
{
    public class AccountProviderUpdateProfileViewModel
    {
        [MaxLength(60, ErrorMessage = "Name length greater than 60")]
        public string CompanyName { get; set; }
        [MaxLength(60, ErrorMessage = "Web site length is greater than 60")]
        public string WebSite { get; set; }
        [MaxLength(100, ErrorMessage = "Address length is greater than 100")]
        public string Address { get; set; }
    }
}
