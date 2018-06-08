using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class LocationHomeViewModel
    {
        public Location[] LocationNsw { get; set; }
        public Location[] LocationInt { get; set; }
        public IEnumerable<Location> locations { get; set; }
    }
}
