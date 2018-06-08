using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }
        public ICollection<Package> Packages { get; set; }
        public bool Active { get; set; }
    }
}
