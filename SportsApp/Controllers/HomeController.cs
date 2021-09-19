using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamService _teamService;
        private readonly ISportFieldService _sportFieldService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ITeamService teamService, UserManager<ApplicationUser> userManager,
        ISportFieldService sportFieldService)
        {
            _sportFieldService = sportFieldService;
            _teamService = teamService;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Teams()
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var teamCurrentUser = await _teamService.GetCurrentUserTeam(currentUser.TeamId);
            if (teamCurrentUser != null)
            {
                ViewData["CurrentUserTeamId"] = teamCurrentUser.Id.ToString();
                ViewData["Categories"] = await _teamService.GetSportCategories();
                ViewData["SportFields"] = await _sportFieldService.GetSportfieldFromCategory(teamCurrentUser.CategoryId);

                return View();
            }
            else
            {
                ViewData["SportFields"] = await _sportFieldService.Get();
                ViewData["CurrentUserTeamId"] = null;
                ViewData["Categories"] = await _teamService.GetSportCategories();
                return View();
            }
        }
        
        //GET TEAMS WITHOUT LOGGED IN USERS TEAM
        [HttpGet]
        public async Task<IActionResult> GetFiltredTeams(string category, string city) {
            ApplicationUser currentUser =  await _userManager.GetUserAsync(User);
            List<TeamViewModel> filtredTeams = await _teamService.GetFiltredTeams(category, city, currentUser.TeamId);
            if (filtredTeams==null)
            {
                return RedirectToAction("Index", "Home");
            }
            return Json(new { data = filtredTeams });
        }

        //GET TEAMS WITH LOGGED IN USERS TEAM
        [HttpGet]
        public async Task<IActionResult> GetAllFiltredTeams()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            var filtredTeams = await _teamService.GetAllFiltredTeams(currentUser.TeamId);
            if (filtredTeams == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return Json(new { data = filtredTeams });
        }

        public async Task<IActionResult> SportFields()
        {
            ViewData["Categories"] = await _teamService.GetSportCategories();
            List<TeamViewModel> teams = await _teamService.Get();

            return View(teams);
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult HowItWorks()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
