using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DushinWebApp.ViewModels
{
    public class LocationUpdateViewModel
    {

        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(60, ErrorMessage = "Name length is greater than 60")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a state")]
        [MaxLength(20, ErrorMessage = "State length is greater than 20")]
        public string State { get; set; }
        [MaxLength(100, ErrorMessage = "Details length is greater than 100")]
        public string Details { get; set; }

        public string Picture { get; set; }
        public bool Active { get; set; }
    }
}
