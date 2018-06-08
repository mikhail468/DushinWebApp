using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class PackageHomeViewModel
    {
        public Package[] PackagesCarousel { get; set; }
        public Package[] PackageNSW { get; set; }
        public Package[] PackageInt { get; set; }
    }
}
