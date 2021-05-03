using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DushinWebApp.Models;
using DushinWebApp.Services;
using DushinWebApp.ViewModels;
using System.Net;

namespace DushinWebApp.Controllers.Api
{
    public class PackageApiController : Controller
    {
        private IDataService<Location> _locationService;
        private IDataService<Package> _packageService;

        public PackageApiController(IDataService<Location> locationService, IDataService<Package> packageService)
        {
            _locationService = locationService;
            _packageService = packageService;
        }
        [HttpGet("api/locations")]
        public JsonResult GetLocations()
        {
            try
            {
                IEnumerable<Location> list = _locationService.GetAll();
                return Json(list);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = ex.Message });
            }
        }
        [HttpGet("api/allpackages")]
        public JsonResult GetAll()
        {
            try
            {
                IEnumerable<Package> list = _packageService.GetAll();
                return Json(list);
            }catch(Exception ex){
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = ex.Message });
            }
        }
        [HttpGet("api/locpackages")]
        public JsonResult GetPackageByCategory(string name)
        {
            try {

                if (name == null)
                {
                    IEnumerable<Package> list = _packageService.GetAll();
                    return Json(list);
                }
                Location loc = _locationService.GetSingle(l => l.Name.ToUpper() == name.ToUpper());
            if (loc != null) { 
               IEnumerable<Package> list = _packageService.Query(p => p.LocationId == loc.LocationId);
               return Json(list);
            }
            else
            {
                    IEnumerable<Package> list = _packageService.GetAll();
                    return Json(list);
                    //return Json(new { message = "cannot find this category" });
                }
             }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = ex.Message });
            }
        }
    }
}