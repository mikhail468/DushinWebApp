using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class LocationDetailsViewModel
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }
        public List<Package> Packages { get; set; }
        public IEnumerable<Package> UnactivePackages { get; set; }
        public int TotalUnactivePac { get; set; }
        public int TotalPackages { get; set; }
        public string Search { get; set; }
    }
}
