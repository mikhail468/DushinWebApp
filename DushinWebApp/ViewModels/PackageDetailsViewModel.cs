using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;

namespace DushinWebApp.ViewModels
{
    public class PackageDetailsViewModel
    {
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Picture { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public string NewFeedback { get; set; }
        public string UserName { get; set; }

    }
}
