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

namespace DushinWebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private IDataService<Feedback> _feedbackService;
        public FeedbackController(UserManager<IdentityUser> managerService, IDataService<Feedback> feedbackService)
        {
            _userManagerService = managerService;
            _feedbackService = feedbackService;
        }

    }
}