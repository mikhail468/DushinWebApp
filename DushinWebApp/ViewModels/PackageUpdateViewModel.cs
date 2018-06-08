using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DushinWebApp.ViewModels
{
    public class PackageUpdateViewModel
    {
        public int PackageId { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(60, ErrorMessage = "Name length is greater than 60")]
        public string LocName { get; set; }
        [Required(ErrorMessage = "Please enter a state")]
        [MaxLength(20, ErrorMessage = "State length is greater than 20")]
        public string LocState { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(60, ErrorMessage = "Name length less than 60")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter price")]
        [MaxLength(10, ErrorMessage = "Name length less than 10")]
        public double Price { get; set; }
        [MaxLength(100, ErrorMessage = "Description length is greater than 100")]
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool Active { get; set; }
    }
}
