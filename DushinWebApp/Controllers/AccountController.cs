using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DushinWebApp.ViewModels;
using DushinWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DushinWebApp.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;

namespace DushinWebApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private SignInManager<IdentityUser> _signInManagerService;
        private RoleManager<IdentityRole> _roleManagerService;
        private IDataService<Profile> _profileDataService;
        private IDataService<ProviderProfile> _providerProfileDataService;
        private IDataService<Order> _orderService;
        private IHostingEnvironment _environment;

        public AccountController(UserManager<IdentityUser> managerService, SignInManager<IdentityUser> signInService,
            RoleManager<IdentityRole> roleManagerService, IDataService<Profile> profileSarvice, IDataService<Order> orderService,
            IHostingEnvironment environment, IDataService<ProviderProfile> providerProfileDataService)
        {
            _userManagerService = managerService;
            _signInManagerService = signInService;
            _roleManagerService = roleManagerService;
            _profileDataService = profileSarvice;
            _orderService = orderService;
            _environment = environment;
            _providerProfileDataService = providerProfileDataService;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateProfile()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            Profile profile = _profileDataService.GetSingle(p => p.UserId == user.Id);
            AccountUpdateProfileViewModel vm = new AccountUpdateProfileViewModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = user.Email,
                Mobile = profile.Mobile,
                Photo = profile.Photo
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateProfile(AccountUpdateProfileViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                Profile profile = _profileDataService.GetSingle(p => p.UserId == user.Id);
                profile.FirstName = vm.FirstName;
                profile.LastName = vm.LastName;
                profile.Mobile = vm.Mobile;
                if (file != null)
                {
                    //check ??
                    if (file.FileName.ToLower().EndsWith("jpg")||file.FileName.ToLower().EndsWith("png")||file.FileName.ToLower().EndsWith("jpeg")) {
                    //upload server path
                    var serverPath = Path.Combine(_environment.WebRootPath, "profilePhotos");
                    //create a folder
                    Directory.CreateDirectory(serverPath);
                    //get the file name
                    string fileName = Path.GetFileName(User.Identity.Name + ".jpg");

                    //stream the file to the srever
                    using (var fileStream = new FileStream(Path.Combine(serverPath,fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    //assign the picture URL to the cat object
                    profile.Photo = fileName;


                     }
                    else {
                         ViewBag.MyMessage = "Picture must be .jpg, .png or .jpeg";
                        return View(vm);
                    }
                }
                else { profile.Photo = vm.Photo;}
                _profileDataService.Update(profile);
                user.Email = vm.Email;
                await _userManagerService.UpdateAsync(user);
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> ProviderUpdateProfile()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            ProviderProfile profile = _providerProfileDataService.GetSingle(p => p.UserId == user.Id);
            AccountProviderUpdateProfileViewModel vm = new AccountProviderUpdateProfileViewModel
            {
                CompanyName = profile.CompanyName,
                WebSite = profile.WebSite,
                Address = profile.Address
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> ProviderUpdateProfile(AccountProviderUpdateProfileViewModel vm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                ProviderProfile profile = _providerProfileDataService.GetSingle(p => p.UserId == user.Id);
                profile.CompanyName = vm.CompanyName;
                profile.WebSite = vm.WebSite;
                profile.Address = vm.Address;
                _providerProfileDataService.Update(profile);
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser(vm.UserName);
                IdentityResult result = await _userManagerService.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    if (await _roleManagerService.FindByNameAsync("Customer") != null)
                    {
                        await _userManagerService.AddToRoleAsync(user, "Customer");
                    }
                    user = await _userManagerService.FindByNameAsync(vm.UserName);
                    Profile profile = new Profile
                    {
                        UserId = user.Id,
                        FirstName = "Please enter...",
                        LastName = "Please enter...",
                        Mobile="Please enter..."
                    };
                    var serverPath = Path.Combine(_environment.WebRootPath, "profilePhotos");
                    //create file name [format MUST be userName.gif -->easy to show in navbar]
                    string fileName = vm.UserName + ".jpg";
                    //stream the file to the server
                    using (var filestream = new FileStream(
                        Path.Combine(serverPath, fileName)
                        , FileMode.Create)
                        )
                    {
                        //create stream for default photo
                        using (var defaultPhoto = new FileStream(Path.Combine(serverPath,"default.jpg"), FileMode.Open))
                            //set default photo as user profile photo
                            await defaultPhoto.CopyToAsync(filestream);
                    }
                    //assign the picture URL to the category object
                    profile.Photo = fileName;
                    _profileDataService.Create(profile);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(vm);
        }
        [HttpGet]
        public IActionResult RegisterProvider()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterProvider(AccountRegisterProviderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser(vm.UserName);
                IdentityResult result = await _userManagerService.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    if (await _roleManagerService.FindByNameAsync("Provider") != null)
                    {
                        await _userManagerService.AddToRoleAsync(user, "Provider");
                    }
                    user = await _userManagerService.FindByNameAsync(vm.UserName);
                    ProviderProfile providerProfile = new ProviderProfile
                    {
                        UserId = user.Id,
                        CompanyName = "please enter",
                        WebSite = "please enter",
                        Address="please enter..."
                    };
                    _providerProfileDataService.Create(providerProfile);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(vm);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            AccountLoginViewModel vm = new AccountLoginViewModel();
            vm.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManagerService.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User or Password incorrect");
                }
            }
            return View(vm);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManagerService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
        public async Task<IActionResult> Orders()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            IEnumerable<Order> list = _orderService.Query(ord => ord.UserId == user.Id);
            AccountOrdersViewModel vm = new AccountOrdersViewModel
            {
                Orders = list
            };
            return View(vm);
        }
        public async Task<string> GetProfilePicture()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            string pic = _profileDataService.GetSingle(p => p.UserId == user.Id).Photo;
            return pic;
        }
        public IActionResult DeniedPackage()
        {
            return View();
        }

    }
}