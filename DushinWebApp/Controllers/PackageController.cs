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
using System.Diagnostics;

namespace DushinWebApp.Controllers
{
    public class PackageController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private IDataService<Location> _locationService;
        private IDataService<Package> _packageService;
        private IDataService<Feedback> _feedbackService;
        private IHostingEnvironment _environment;
        public PackageController(UserManager<IdentityUser> managerService, IDataService<Location> locationService, 
            IDataService<Package> packageService, IHostingEnvironment environment, IDataService<Feedback> feedbackService)
        {
            _locationService = locationService;
            _packageService = packageService;
            _environment = environment;
            _userManagerService = managerService;
            _feedbackService = feedbackService;
        }
        public IActionResult AllPackages()
        {
            IEnumerable<Package> list = _packageService.GetAll().Where(p=>p.Active==true);
           
            PackageHomeViewModel vm = new PackageHomeViewModel
            {
                PackagesCarousel = new Package[3],
                PackageNSW = new Package[3],
                PackageInt = new Package[3]
            };
            int i = 0;
            foreach (var item in list)
            {
                if (i == 3) break;
                if (item.Active)
                {
                    vm.PackagesCarousel[i] = item;
                    i++;
                }
            }
            i = 0;
            foreach (var item in list)
            {
                if (i == 3) break;
                if (item.Active && item.LocState.ToUpper() == "NSW")
                {
                    vm.PackageNSW[i] = item;
                    i++;
                }
            }
            i = 0;
            foreach (var item in list)
            {
                if (i == 3) break;
                if (item.Active && item.LocState.ToUpper() != "NSW")
                {
                    vm.PackageInt[i] = item;
                    i++;
                }
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string name)
        {
            Package pac = _packageService.GetSingle(p => p.Name == name);
            List<Feedback> list = _feedbackService.Query(f => f.PackageId == pac.PackageId).OrderBy(f=>f.ComntDate).ToList();
            IdentityUser user = await _userManagerService.FindByIdAsync(pac.UserId);
            PackageDetailsViewModel vm = new PackageDetailsViewModel
            {
                Name = pac.Name,
                LocationName = _locationService.GetSingle(l=>l.LocationId==pac.LocationId).Name,
                Price = pac.Price,
                Description = pac.Description,
                Picture = pac.Picture,
                UserName = user.UserName,
                Feedbacks=list
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Details(PackageDetailsViewModel vm)
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            Package package = _packageService.GetSingle(p => p.Name == vm.Name);
            Feedback feedback = new Feedback
            {
                UserId = user.Id,
                PackageId = package.PackageId,
                ComntDate = DateTime.Now,
                Comment = vm.NewFeedback,
                UserName = User.Identity.Name
            };
            _feedbackService.Create(feedback);
            return RedirectToAction("Details", "Package",new {name=vm.Name });
        }
        [HttpGet]
        [Authorize(Roles = "Provider")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Create(PackageCreateViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                //check exists - Name = UNQ
                Package existingPackage = _packageService.GetSingle(p => p.Name == vm.Name);
                Location loc = _locationService.GetSingle(l => l.LocationId == int.Parse(TempData["locId"].ToString()));
                if (existingPackage == null)
                {
                    Package pac = new Package
                    {
                        LocationId = loc.LocationId,
                        Name = vm.Name,
                        LocName = loc.Name,
                        Price=vm.Price,
                        Description = vm.Description,
                        Active=vm.Active,
                        LocState = loc.State,
                        UserId = user.Id

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
                        pac.Picture = User.Identity.Name + "/" + fileName;
                    }

                    //save to db
                    _packageService.Create(pac);
                    //go home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.MyMessage = "Package name exists. Please change the name";
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Update(string name)
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            
            Package pac = _packageService.GetSingle(p => p.Name == name);
            if (pac.UserId == user.Id) { 
            PackageUpdateViewModel vm = new PackageUpdateViewModel
            {
                PackageId=pac.PackageId,
                Name = pac.Name,
                LocName = pac.LocName,
                Description = pac.Description,
                Price = pac.Price,
                Picture = pac.Picture,
                LocState = pac.LocState,
                Active = pac.Active
            };
                return View(vm);
            }
            else
            {
                return RedirectToAction("DeniedPackage", "Account");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Update(PackageUpdateViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Package package = _packageService.GetSingle(p=>p.Name==vm.Name);
                package.Name = vm.Name;
                package.LocName = vm.LocName;
                package.Price = vm.Price;
                package.Description = vm.Description;
                package.LocState = vm.LocState;
                package.Active = vm.Active;
                    //Package pac = new Package
                    //{
                    //    LocationId = int.Parse(TempData["locId"].ToString()),
                    //    Name = vm.Name,
                    //    Price = vm.Price,
                    //    Description = vm.Description,
                    //    Active = vm.Active,
                    //    Picture=vm.Picture
                    //};
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
                        package.Picture = User.Identity.Name + "/" + fileName;
                        //string oldPicturePath = "~/uploads/" +  vm.Picture;
                        //FileInfo myfileinf = new FileInfo(oldPicturePath);
                        //myfileinf.Delete();
                    }

                    //save to db
                    _packageService.Update(package);
                
                //go home
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(vm);
            }
        }

        public async Task<IActionResult> ProviderPackages()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            IEnumerable<Package> list = _packageService.Query(p => p.UserId == user.Id);
            AccountProviderPackagesViewModel vm = new AccountProviderPackagesViewModel
            {
                Total = list.Count(),
                Packages = list
            };
            return View(vm);
        }
    }
}