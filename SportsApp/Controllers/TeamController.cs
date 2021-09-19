using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{

    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ICategoryService _categoryService;

        private readonly UserManager<ApplicationUser> _userManager;

        public TeamController(ITeamService teamService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _teamService = teamService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        // GET: Team
        [Authorize]
        public async Task<IActionResult> AllTeamsList()
        {
            return View(await Get());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Json(new { data = await _teamService.Get() });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyTeamsRequest(Guid teamId)
        {
            return Json(new { data = await _teamService.GetMyTeamRequests(teamId) });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllFiltredTR()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return Json(new { data = await _teamService.GetMyJoinRequests(Guid.Parse(currentUser.Id))});
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFiltredTR(string category,string city)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return Json(new { data = await _teamService.GetMyJoinRequestFiltred(Guid.Parse(currentUser.Id),category,city)});
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UnsendRequest(string teamId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string msg = await _teamService.DropOneRequest(currentUser.Id,teamId);
            bool result = bool.Parse(msg.Split(":")[0]);
            string msgActual = msg.Split(":")[1];
            return Json(new { success = result,message = msgActual });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Requests() {
            ViewData["categorys"] = await _categoryService.Get();
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyInvitation()
        {
            return Json(new { data = await _teamService.Get() });
        }
        // GET: Team/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            if(id == Guid.Empty)
            {
                return RedirectToAction("Index", "Home");
            }
            Team team = await _teamService.Get(id);
            if (team == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ViewData["CurrentUserName"] = user.UserName;
            ViewData["isFull"] = await _teamService.IsTeamFull(id);
            ViewData["isJoinedInATeam"] = user.TeamId != null;
            ViewData["isJoined"] = user.TeamId == id;
            ViewData["isLeader"] = await _teamService.PlayerIsLeader(id, Guid.Parse(_userManager.GetUserId(User)));
            ViewData["leaderName"] = await _teamService.GetTeamLeaderUserName(id);
            ViewData["players"] = await _teamService.GetTeamPlayers(id);
            ViewData["AlreadyRequested"] = await _teamService.CheckAlreadyRequested(id,user.Id);
            ViewData["categoryName"] = await _teamService.GetSportCategoryName(team.CategoryId);
            ViewData["PlayersRequests"] = await _teamService.GetMyTeamRequests(id);

            return View(team);
        }

        public async Task<IActionResult> JoinNotificationIsRead(Guid id,string notificationId)
        {
            string type = await _teamService.NotificationTypeCheck(notificationId);
            await _teamService.JoinNotificationIsRead(notificationId);
            if(type == "TeamInvite")
            {
                return RedirectToAction("Details", new { id = id });
            }
            else if(type == "MatchInvite") 
            {
                return RedirectToAction("AcceptMatch", "Matches");
            }
            else
            {
                return RedirectToAction("Index","Home"); 
            }
        }
        public async Task<IActionResult> JoinNotificationAlreadyRead(Guid id, string notificationId)
        {
            string type = await _teamService.NotificationTypeCheck(notificationId);
            if (type == "TeamInvite")
            {
                return RedirectToAction("Details", new { id = id });
            }
            else if (type == "MatchInvite")
            {
                return RedirectToAction("AcceptMatch", "Matches");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Team/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel.Categories = await _teamService.GetSportCategories();
            return View(teamViewModel);
        }

        // POST: Team/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string teamName,string city,string categoryId)
        {
            TeamViewModel model = new TeamViewModel
            {
                Name = teamName,
                City = (Cities)Enum.Parse(typeof(Cities), city),
                CategoryId = Guid.Parse(categoryId),
                isPrivate = true
            };
            string teamLeaderId = _userManager.GetUserId(User);
            if (ModelState.IsValid && !(await _teamService.PlayerHasATeam(Guid.Parse(teamLeaderId))))
            { 
                model.TeamLeaderId = teamLeaderId;
                bool result = await _teamService.DropAllPlayerRequests(teamLeaderId);
                if (result)
                {
                    Team createdTeam = await _teamService.Create(model);
                    if (createdTeam.Name.StartsWith("Err42c0k7"))
                    {
                        var Errmessage = createdTeam.Name.Split(":")[1];
                        return Json(new { success = false, message = Errmessage }); ;
                    }
                    else
                    {
                        return Json(new { success = true, message = "Team created successfully! Shortly you will be redirected!", teamId = createdTeam.Id});
                    }
                }
            }
            return RedirectToAction(nameof(CreateJoinUnsuccessful));
        }
        public IActionResult CreateJoinUnsuccessful()
        {
            return View();
        }

        // GET: Team/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            Team team = await _teamService.Get(id);
            if (team == null)
            {
                return NotFound();
            }
            if (user.Id == team.TeamLeaderId.ToString() || user.Role == "Admin")
            {
                TeamViewModel teamViewModel = new TeamViewModel
                {
                    Name = team.Name,
                    City = team.City,
                    TeamLeaderId = team.TeamLeaderId.ToString(),
                    isPrivate = team.isPrivate
                };
                ViewData["Id"] = id;
                return View(teamViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TeamViewModel model, Guid id)
        {
            if (ModelState.IsValid)
            {
                await _teamService.Update(model, id);
                return RedirectToAction("Details", new {id = id});
            }
            return View(viewName: "Edit");
        }

        // GET: Team/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            Team team = await _teamService.Get(id);
            if (team == null)
            {
                return Json(new { success = false, message = "Delete Unsuccessful!" });
            }
            bool result = false;
            if (user.Id == team.TeamLeaderId.ToString() || user.Role == "Admin")
            {
                result = await _teamService.Delete(id);
            }
            return Json(new { success = result, message = result ? "Delete Successful!" : "Delete Unsuccessful!" });
        }

        [HttpGet]
        public async Task<IActionResult> JoinInvitation(Guid id, string notificationId)
        {
            string playerId = _userManager.GetUserId(User);
            string resultAsString = await _teamService.JoinInvitation(id, Guid.Parse(playerId));
            bool result = bool.Parse(resultAsString.Split(":")[0]);
            if (result)
            {
                var status = await _teamService.DropAllNotifications(playerId);
                if (status)
                {
                    result = status;
                }
                else
                {
                    return Json(new { success = status, message = "Something Failed!" });
                }
            }
            return Json(new { success = result, message = resultAsString.Split(":")[1] });
        }
        
        [HttpGet]
        public async Task<JsonResult> RequestToJoin(Guid id)
        {
            string playerId = _userManager.GetUserId(User);
            var result = await _teamService.Request(id, Guid.Parse(playerId));
            string msg= "";
            bool rslt= false;
            if (result != null) {
                foreach (KeyValuePair<bool, string> x in result)
                {
                    msg = x.Value;
                    rslt = x.Key;
                }
                return Json(new { success = rslt, message = msg });
            }
            else {
                return Json(new { success = false, message = "Something failed!"});
            }
        }

        [HttpGet]
        public async Task<JsonResult> Join(Guid id)
        {
            string playerId = _userManager.GetUserId(User);
            string resultAsString = await _teamService.Join(id, Guid.Parse(playerId));
            bool result = bool.Parse(resultAsString.Split(":")[0]);
            if (result)
            {
                return Json(new { success = result, message = resultAsString.Split(":")[1] });
            }
            else
            {
                return Json(new { success = result, message = resultAsString.Split(":")[1] });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ApproveRequest(Guid id,Guid playerId)
        {
            var result = await _teamService.ApproveRequest(id,playerId);

            return Json(new { success = result, message = result ? "The player was added to your team successfully!" : "The player wasn't added cause he might have cancelled the request !" });
        }
        [HttpDelete]
        public async Task<JsonResult> RefuseRequest(Guid id, Guid playerId)
        {
            var result = await _teamService.RefuseRequest(id, playerId);

            return Json(new { success = result, message = result ? "The Player request was refused successfully!" : "The player request wasn't refused because the player already removed the request!" });
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveFromTeam(string playerId,string teamId)
        {
            ApplicationUser curretUser = await _userManager.GetUserAsync(User);
            ApplicationUser player = await _userManager.FindByIdAsync(playerId);
            string teamleadername = await _teamService.GetTeamLeaderUserName(Guid.Parse(teamId));
            if (curretUser.UserName != teamleadername)
            {
                return Json(new { success = false, message = "You don't have permission to remove players!" });
            }
            if (player == null)
            {
                return Json(new { success = false, message = "This player doesn't exist players!" });
            }
            bool result = await _teamService.Leave(Guid.Parse(teamId),Guid.Parse(playerId));
            return Json(new { success = result, message = result ? "Player removed successfully!" : "The player couldn't be removed!" });
        }

        public async Task<IActionResult> Leave(Guid id)
        {
            string playerId = _userManager.GetUserId(User);
            bool result = await _teamService.Leave(id, Guid.Parse(playerId));
            return Json(new { success = result, message = result ? "You have left the team!" : "Some error occurred!" });
        }

        public IActionResult LeaveTeamUnsuccessful()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> InviteForJoin(string username)
        {
            string resultAsString = "False:Invite Unsuccessfull";
            if (_userManager.GetUserName(User) != username)
            {
                Guid? teamId = (await _userManager.GetUserAsync(User)).TeamId ?? null;
                if (teamId.HasValue)
                {
                    var teamName = (await _teamService.Get(teamId.Value)).Name;
                    resultAsString = await _teamService.InviteForJoin(teamName, username);
                }
            }
            bool result = bool.Parse(resultAsString.Split(":")[0]);
            return Json(new { success = result , message = resultAsString.Split(":")[1] });
        }

        [HttpPost]
        public async Task<JsonResult> UsernameAutoComplete(string usernamePrefix)
        {
            var currentUser = (await _userManager.GetUserAsync(User)).UserName;
            List<string> usernames =  await _teamService.UsernameAutoComplete(usernamePrefix,currentUser);
            return Json(new { data = usernames });
        }
        public async Task<IActionResult> ChangeAccess(Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            bool result = await _teamService.ChangeAccess(id);
            if (await _teamService.IsTeamPrivate(id))
            {
                return Json(new { success = result, message = result ? "Your Team is now Private!" : "Your Team can't be Private!" });
            }
            return Json(new { success = result, message = result ? "Your Team is now Public!" : "Your Team can't be Public!" });
        }
    }
}
