using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class AccountProviderPackagesViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}
