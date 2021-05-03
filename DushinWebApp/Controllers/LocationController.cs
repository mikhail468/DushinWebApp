using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DushinWebApp.Models;
using DushinWebApp.Services;
using DushinWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DushinWebApp.Controllers
{
    public class LocationController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private IDataService<Location> _locationService;
        private IDataService<Package> _packageService;
        private IHostingEnvironment _environment;
        public LocationController(UserManager<IdentityUser> managerService, IDataService<Location> locationService,
            IDataService<Package> packageService,
            IHostingEnvironment environment)
        {
            _userManagerService = managerService;
            _locationService = locationService;
            _packageService = packageService;
            _environment = environment;
        }
        public IActionResult Home()
        {
            IEnumerable<Location> list = _locationService.GetAll().Where(l => l.Active == true);
            LocationHomeViewModel vm = new LocationHomeViewModel
            {
                locations=list,
                LocationNsw = new Location[3],
                LocationInt = new Location[3]
            };
            int i = 0;
            foreach (var item in list)
            {
                if (i == 3) break;
                if (item.Active&&item.State=="NSW")
                {
                    vm.LocationNsw[i] = item;
                    i++;
                }
            }
            i = 0;
            foreach (var item in list)
            {
                if (i == 3) break;
                if (item.Active && item.State != "NSW")
                {
                    vm.LocationInt[i] = item;
                    i++;
                }
            }
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string name,string sorting)
        {
            IEnumerable<Package> list = _packageService.GetAll();
            Location loc = _locationService.GetSingle(l => l.Name == name);
            if (loc != null) {
                TempData["locId"] = loc.LocationId.ToString();
                List<Package> packageList;
                switch (sorting)
                {
                    case "hight": packageList = list.Where(p => p.LocationId == loc.LocationId).Where(p => p.Active == true).OrderByDescending(x=>x.Price).ToList();break;
                    case "low": packageList= list.Where(p => p.LocationId == loc.LocationId).Where(p => p.Active == true).OrderBy(x => x.Price).ToList(); break;
                    default : packageList = list.Where(p => p.LocationId == loc.LocationId).Where(p => p.Active == true).ToList();break;
                }
                LocationDetailsViewModel vm = new LocationDetailsViewModel
                {
                    LocationId = loc.LocationId,
                    Name = loc.Name,
                    Details = loc.Details,
                    TotalPackages = packageList.Count(),
                    Packages = packageList,
                    Picture =loc.Picture
                };
                if(User.Identity.IsAuthenticated && User.IsInRole("Provider"))
                {
                    IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                    vm.UnactivePackages = list.Where(p => p.LocationId == loc.LocationId).Where(p => p.UserId == user.Id).Where(p => p.Active == false);
                    vm.TotalUnactivePac = list.Where(p => p.LocationId == loc.LocationId).Where(p => p.UserId == user.Id).Where(p => p.Active == false).Count();
                }
                return View(vm);
            }
            else
            {
                List<Package> packageList;
                switch (sorting)
                {
                    case "hight": packageList = _packageService.GetAll().Where(p => p.Active == true).OrderByDescending(x => x.Price).ToList(); break;
                    case "low": packageList = _packageService.GetAll().Where(p => p.Active == true).OrderBy(x => x.Price).ToList(); break;
                    default: packageList = _packageService.GetAll().Where(p => p.Active == true).ToList(); break;
                }
                LocationDetailsViewModel vm = new LocationDetailsViewModel
                {
                    TotalPackages = packageList.Count(),
                    Packages = packageList
                };
                if (User.Identity.IsAuthenticated && User.IsInRole("Provider"))
                {
                    IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                    vm.UnactivePackages = list.Where(p => p.UserId == user.Id).Where(p => p.Active == false);
                    vm.TotalUnactivePac = list.Where(p => p.UserId == user.Id).Where(p => p.Active == false).Count();
                }
                return View(vm);
            }
        }
        [HttpPost]
        public IActionResult Details(LocationDetailsViewModel vm)
        {
            return RedirectToAction("Details", "Location", new { name = vm.Search });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LocationCreateViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //check exists - Name = UNQ
                Location existingLocation = _locationService.GetSingle(loc => loc.Name == vm.Name);
                if (existingLocation == null)
                {
                    Location loc = new Location
                    {
                        Name = vm.Name,
                        Details = vm.Details,
                        Active = vm.Active,
                        State = vm.State
                    };
                    //upload the picture to the file system
                    //assign the picture URL to the cat object
                    if (file != null)
                    {
                        //check ??

                        //upload server path
                        var serverPath = Path.Combine(_environment.WebRootPath, "uploads");
                        //create a folder
                        Directory.CreateDirectory(Path.Combine(serverPath, User.Identity.Name));
                        //get the file name
                        string fileName = FileNameHelper.GetNameFormated(Path.GetFileName(file.FileName));

                        //stream the file to the srever
                        using (var fileStream = new FileStream(Path.Combine(serverPath, User.Identity.Name, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //assign the picture URL to the cat object
                        loc.Picture = User.Identity.Name + "/" + fileName;
                    }

                    //save to db
                    _locationService.Create(loc);
                    //go home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.MyMessage = "Location name exists. Please change the name";
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(string name)
        {
            Location loc = _locationService.GetSingle(l => l.Name == name);
            LocationUpdateViewModel vm = new LocationUpdateViewModel
            {
                Name = loc.Name,
                Details = loc.Details,
                Active = loc.Active,
                State = loc.State,
                Picture = loc.Picture
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(LocationUpdateViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Location location = _locationService.GetSingle(l => l.Name == vm.Name);
                location.Name = vm.Name;
                location.State = vm.State;
                location.Details = vm.Details;
                location.Active = vm.Active;
                    if (file != null)
                    {
                        //check ??

                        //upload server path
                        var serverPath = Path.Combine(_environment.WebRootPath, "uploads");
                        //create a folder
                        Directory.CreateDirectory(Path.Combine(serverPath, User.Identity.Name));
                        //get the file name
                        string fileName = FileNameHelper.GetNameFormated(Path.GetFileName(file.FileName));

                        //stream the file to the srever
                        using (var fileStream = new FileStream(Path.Combine(serverPath, User.Identity.Name, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //assign the picture URL to the cat object
                        location.Picture = User.Identity.Name + "/" + fileName;
                    }

                //save to db
                _locationService.Update(location);
                    //go home
                    return RedirectToAction("Details", "Location", new { name = vm.Name });
            }
            else
            {
               ViewBag.MyMessage = "Location name exists. Please change the name";
               return View(vm);
            }
        }
    }
}