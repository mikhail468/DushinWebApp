using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.ViewModels
{
    public class OrderPurchaseViewModel
    {
        public string Name { get; set; }
        public DateTime Date {get;set;}
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
