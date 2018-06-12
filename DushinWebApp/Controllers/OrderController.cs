using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DushinWebApp.Models;
using DushinWebApp.Services;
using DushinWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace DushinWebApp.Controllers
{
    public class OrderController : Controller
    {
        private IDataService<Order> _orderService;
        private IDataService<Package> _packageService;
        private UserManager<IdentityUser> _userManagerService;
        public OrderController(IDataService<Order> orderService,IDataService<Package> packageService,
                                UserManager<IdentityUser> managerService)
        {
            _orderService = orderService;
            _packageService = packageService;
            _userManagerService = managerService;
        }
        [HttpGet]
        public IActionResult Purchase(string name)
        {
            Package pac = _packageService.GetSingle(p => p.Name == name);
            OrderPurchaseViewModel vm = new OrderPurchaseViewModel
            {
                Name = pac.Name,
                Date = DateTime.Now,
                Price = pac.Price
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Purchase(OrderPurchaseViewModel vm)
        {
            Package pac = _packageService.GetSingle(p => p.Name == vm.Name);
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            Order ord = new Order
            {
                UserId = user.Id,
                Price = vm.Price,
                Name = vm.Name,
                Date = vm.Date
            };
            _orderService.Create(ord);
            pac.TimesOrdered++;
            _packageService.Update(pac);
            return RedirectToAction("Details", "Package", new { name = vm.Name });
        }
    }
}