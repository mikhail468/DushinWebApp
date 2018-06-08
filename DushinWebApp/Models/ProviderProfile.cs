using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Models
{
    public class ProviderProfile
    {
        public int ProviderProfileId { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
    }
}
