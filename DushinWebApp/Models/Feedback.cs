using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int PackageId { get; set; }
        public DateTime ComntDate { get; set; }
        public string Comment { get; set; }
    }
}
