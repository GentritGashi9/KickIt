using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{
    [Authorize]
    public class MatchesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMatchService _context;
        private readonly ISportFieldService sportFieldService;

        public MatchesController(UserManager<ApplicationUser> userManager, IMatchService context, ISportFieldService sportFieldService)
        {
            _context = context;
            _userManager = userManager;
            this.sportFieldService = sportFieldService;
        }

        [BindProperty]
        public ModelFile Input { get; set; }
        public class ModelFile
        {
            public IFormFile File { get; set; }
        }
        [HttpGet]
        public ActionResult ChatRoom()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MatchDetails(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var match = await _context.GetMatch(Guid.Parse(id));
            ViewData["SportFields"] = await sportFieldService.GetSpecificSportField(Guid.Parse(match.SportFieldId));
            ViewData["Schedule"] = await _context.GetMatchSchedule(id);
            ViewData["Team1"] = await _context.GetTeam(match.Team1Id);
            ViewData["Team2"] = await _context.GetTeam(match.Team2Id);
            ViewData["Match"] = match;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Match()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["CurrentUserTeamId"] = await _context.CurrentUserTeamId(user);
            ViewData["SportFields"] = await sportFieldService.Get();
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Teams()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new { data = await _context.Match(user) });
        }
        [HttpPost]
        public async Task<JsonResult> AskForMatch(string team1Id,
         string team2Id, string sportFieldId, string startTime, string endTime)
        {
            var resultCheck = await _context.MatchExists(team1Id, team2Id, sportFieldId);
            var resultCheckActual = resultCheck.Split(":")[0];
            var messageCheckActual = resultCheck.Split(":")[1];
            if (resultCheckActual.Equals("True"))
            {
                var startT = DateTime.Parse(startTime);
                var endT = DateTime.Parse(endTime);
                bool reserveResult = await _context.ReserveSchedule(sportFieldId, startT, endT);
                if (reserveResult)
                {
                    var result = await _context.AskForMatch(team1Id, team2Id, sportFieldId, startT, endT);
                    var resultActual = result.Split(":")[0];
                    var messageActual = result.Split(":")[1];
                    if (resultActual.Equals("True"))
                    {
                        bool notifyresult = await _context.NotificationAskForMatch(team2Id, team1Id);
                        if (notifyresult)
                        {
                            return Json(new { success = true, message = messageActual });
                        }
                        return Json(new { success = false, message = "The Notification wasn't sended successfully!" });

                    }
                    else
                    {
                        return Json(new { success = false, message = messageActual });
                    }

                }
                else
                {
                    return Json(new { success = false, message = "The Schedule was unable to be reserved,it might have been already reserved!" });
                }
            }
            return Json(new { success = false, message = "Error requesting to play a game! " + messageCheckActual });
        }
        [HttpGet]
        public ActionResult AcceptMatch()
        {
            return View();
        }
      

        [HttpGet]
        public async Task<JsonResult> Pending()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new { data = await _context.AcceptMatch(user.TeamId.ToString()) });
        }
        [HttpGet]
        public async Task<IActionResult> AcceptForMatchC(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var team = await _context.GetTeam(user.TeamId.ToString());
            if (team.TeamLeaderId.ToString() != user.Id) { return Json(new { success = false, message = "Only your TeamLeader can accept match requests!" }); }
            var gameRoom = await _context.AcceptForMatch(id);
            var schedule = await _context.GetMatchSchedule(id);
            if (gameRoom.Name.StartsWith("Err30a1e6"))
            {
                var Errmessage = gameRoom.Name.Split(":")[1];
                return Json(new { success = false, message = Errmessage });
            }
            else
            {
                return Json(new { success = true, message = "Request accepted successfully! Shortly you will be redirected!", gameRoomId = gameRoom.Id, startTime = schedule.StartTime, endTime = schedule.EndTime });
            }
        }
        [HttpGet]
        public async Task<IActionResult> AcceptForMatch(string id, string startTime, string endTime)
        {
            var gameRoom = await _context.GetGameRoom(id);
            ViewData["gameRoom"] = gameRoom;
            var teams = await _context.GetGameRoomTeams(id);
            ViewData["Teams"] = teams;
            ViewData["EndTime"] = DateTime.Parse(endTime);
            ViewData["StartTime"] = DateTime.Parse(startTime);

            ViewData["T1Id"] = teams[0].Id.ToString();
            ViewData["T2Id"] = teams[1].Id.ToString();
            return View();

        }
        [HttpDelete]
        public async Task<JsonResult> RefuseForMatch(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var team = await _context.GetTeam(user.TeamId.ToString());
            if (team.TeamLeaderId.ToString() != user.Id) { return Json(new { success = false, message = "Only your TeamLeader can refuse match requests!" }); }
            bool scheduleResult = await _context.FreeSchedule(id);
            if (scheduleResult)
            {
                var result = await _context.RefuseForMatch(id);
                if (result == false)
                {
                    return Json(new { success = false, message = "Error while refusing match! This match doesn't exist!" });
                }
                return Json(new { success = true, message = "Match request successfully refused!." });
            }
            return Json(new { success = false, message = "Error while refusing match! Schedule wasn't made free!" });
        }
    }
}
