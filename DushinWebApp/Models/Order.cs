using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
