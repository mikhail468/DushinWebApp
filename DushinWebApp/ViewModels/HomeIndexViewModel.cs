using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public string locationName { get; set; }
        public Location[] Locations { get; set; }
        public Package[] Packages { get; set; }
    }
}
