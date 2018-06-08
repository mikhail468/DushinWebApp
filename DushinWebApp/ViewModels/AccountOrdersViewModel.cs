using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class AccountOrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
