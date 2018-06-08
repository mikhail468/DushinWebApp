using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DushinWebApp.Models;
using DushinWebApp.Services;

namespace DushinWebApp.Controllers
{
    //[Produces("application/json")]
    //[Route("api/Api")]
    public class ApiController : Controller
    {
        private IDataService<Package> _packageService;
        private IDataService<Location> _locationService;
        public ApiController(IDataService<Location> locationService,
            IDataService<Package> packageService)
        {
            _locationService = locationService;
            _packageService = packageService;
        }
        
    }
}