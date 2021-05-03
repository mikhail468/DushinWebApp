using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DushinWebApp.Models;
using DushinWebApp.Services;
using DushinWebApp.ViewModels;
using System.Diagnostics;

namespace DushinWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IDataService<Location> _locationService;
        private IDataService<Package> _packageService;
        public HomeController(IDataService<Location> locationService, IDataService<Package> packageService)
        {
            _locationService = locationService;
            _packageService = packageService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Location> locationList = _locationService.GetAll();
            IEnumerable<Package> packageList = _packageService.GetAll();
            HomeIndexViewModel vm = new HomeIndexViewModel
            {
                Locations = new Location[6],
                Packages = new Package[6]
            };
            int i = 0;
            foreach (var item in locationList)
            {
                if (i == 6) break;
                if (item.Active)
                {
                    vm.Locations[i] = item;
                    i++;
                }
            }
            i = 0;
            foreach (var item in packageList)
            {
                if (i == 6) break;
                if (item.Active)
                {
                    vm.Packages[i] = item;
                    i++;
                }
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(HomeIndexViewModel vm)
        {
            return RedirectToAction("Details","Location",new { name = vm.locationName });
        }
        public string[] GetAllLocations(string term)
        {

            string[] locations = _locationService.Query(x => x.Name.ToLower().StartsWith(term.ToLower())).Select(y=>y.Name).ToList().ToArray();
            
            return locations;
        }
        public IActionResult Packages()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        
    }
}