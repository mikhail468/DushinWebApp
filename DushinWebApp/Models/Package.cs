using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Models
{
    public class Package
    {
        public int PackageId { get; set; }
        public string Name { get; set; }
        public string LocName { get; set; }
        public string LocState { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string Picture { get; set; }
        public bool Active { get; set; }
        public string UserId { get; set; }
    }
}
