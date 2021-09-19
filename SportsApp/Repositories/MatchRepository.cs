using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment environment;
        private readonly ITeamService _teamService;


        public MatchRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment _environment, ITeamService teamService)
        {
            _context = context;
            _userManager = userManager;
            environment = _environment;
            _teamService = teamService;
        }

        public async Task<string> CurrentUserTeamId(ApplicationUser user)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(u => u.Id == user.TeamId);
            if (team == null) { return ""; }
            return team.Id.ToString();
        }
        public async Task<List<Team>> Match(ApplicationUser user)
        {
            return await _context.Teams.Where(u => u.Id != user.TeamId).ToListAsync();
        }
        public async Task<GameRoom> GetGameRoom(string id)
        {
            return await _context.GameRooms.FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));
        }
        public async Task<List<Team>> GetGameRoomTeams(string id)
        {
            return await _context.Teams.Where(u => u.GameRoomId == Guid.Parse(id)).ToListAsync();
        }
        public async Task<string> MatchCheck(string team1Id, string team2Id, string sportFieldsId)
        {
            if (team1Id == null || team2Id == null) { return "False:You are not part of a Team!"; }

            var team1 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team1Id));
            var team2 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team2Id));

            var sportField = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == Guid.Parse(sportFieldsId));

            if (team1 == null || team2 == null) { return "False:This Adversary Team or your Team doesn't exist anymore!"; }
            if (team1.CategoryId != team2.CategoryId) { return "False:Your team sport category is different from you adversary's!"; }
            if (team1.CategoryId != sportField.CategoryId || team2.CategoryId != sportField.CategoryId) { return "False:You can't chose this field because the field can host a different sport category!"; }
            int team1Players = await _teamService.GetNumberOfPlayers(Guid.Parse(team1Id));
            int team2Players = await _teamService.GetNumberOfPlayers(Guid.Parse(team2Id));

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == sportField.CategoryId);
            if (team1Players < category.MinCapacity || team2.Players.Count() < category.MinCapacity)
            {
                return "False:Your Team or the Adversary Team doesn't have enough players!";
            }

            var existsMatch = await _context.Matches.FirstOrDefaultAsync(x => (x.Team1Id == team1Id && x.Team2Id == team2Id) ||
                                                                              (x.Team1Id == team2Id && x.Team2Id == team1Id));
            var existMatchAccepted = await _context.Matches.FirstOrDefaultAsync(x => ((x.Team1Id == team2Id || x.Team2Id == team2Id) ||
                                                                                      (x.Team1Id == team1Id || x.Team2Id == team1Id))
                                                                                                                && x.IsAccepted == true);

            if (existsMatch != null) { return "False:You already requested, or got invited, to play with this team!"; }
            if (existMatchAccepted != null) { return "False:You already have accepted to play with another team or the adversary team already accepted another team!"; }
            return "True:Check Successfull.";
        }
        public async Task<string> AskForMatch(string team1Id, string team2Id, string sportFieldsId, DateTime startTime, DateTime endTime)
        {
            var team1 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team1Id));
            var team2 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(team2Id));
            var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.SportFieldId == Guid.Parse(sportFieldsId) && x.StartTime == startTime && x.EndTime == endTime);

            var match = new Matches
            {
                Id = Guid.NewGuid(),
                Name = team1.Name + " vs " + team2.Name,
                Team1Id = team1Id,
                Team2Id = team2Id,
                SportFieldId = sportFieldsId,
                Schedule = schedule,
                IsAccepted = false
            };


            var result = _context.Matches.AddAsync(match);
            if (result.IsCompletedSuccessfully)
            {
                await _context.SaveChangesAsync();
            }
            schedule.MatchesId = match.Id;
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();

            return "True:Request to play a game successfully send! Please wait for adversary team Response.";
        }

        public async Task<List<Matches>> AcceptMatch(string teamId)
        {
            return await _context.Matches.Where(x => x.Team2Id == teamId && x.IsAccepted == false).ToListAsync();
        }

        public async Task<bool> RefuseForMatch(string matchId)
        {
            if (matchId == null) { return false; }
            var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == Guid.Parse(matchId));
            if (match == null) { return false; }
            var result = _context.Matches.Remove(match);
            if (result.State.ToString().Equals("Deleted"))
            {
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<GameRoom> AcceptForMatch(string matchId/*,IFormFile img*/)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == Guid.Parse(matchId));
            var team1 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(match.Team1Id));
            var team2 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(match.Team2Id));
            var existMatchAccepted = await _context.Matches.FirstOrDefaultAsync(x => ((x.Team1Id == team1.Id.ToString() && x.Team2Id != team2.Id.ToString()) ||
                                                                                     (x.Team1Id != team1.Id.ToString() && x.Team2Id == team2.Id.ToString()) ||
                                                                                     (x.Team1Id != team2.Id.ToString() && x.Team2Id == team1.Id.ToString()) ||
                                                                                     (x.Team1Id == team2.Id.ToString() && x.Team2Id != team1.Id.ToString())) &&
                                                                                     x.IsAccepted == true);
            if (existMatchAccepted != null)
            {
                return new GameRoom
                {
                    Name = "Err30a1e6:Cannot Accept the invitation cause you are " +
                                                      "already accepted to play with another team or adversary team !"
                };
            }

            match.IsAccepted = true;
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();

            var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.MatchesId == match.Id);
            schedule.Reserved = true;
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();

            bool resultMatchRequestDrop = await DropAllMatchRequests(team1.Id, team2.Id);

            var notifications = await _context.Notifications.Where(x => x.Type == "MatchInvitation" && (x.UserId == team1.TeamLeaderId.ToString() || x.UserId == team2.TeamLeaderId.ToString())).ToListAsync();
            var status = "";
            if (notifications != null)
            {
                foreach (Notification n in notifications)
                {
                    var statusc = _context.Notifications.Remove(n);
                    status = statusc.State.ToString();
                }
                if (status.Equals("Deleted")) { await _context.SaveChangesAsync(); }
            }
            var gameRoom = new GameRoom
            {
                Id = Guid.NewGuid(),
                Name = team1.Name + " vs " + team2.Name,
                Teams = new List<Team>(),
                //ChatImg = ImgName,
                Matches = match
            };

            gameRoom.Teams.Add(team1);
            gameRoom.Teams.Add(team2);
            await _context.GameRooms.AddAsync(gameRoom);
            //uploadImg(img);
            team1.GameRoomId = gameRoom.Id;
            team1.MatchId = match.Id.ToString();
            team2.GameRoomId = gameRoom.Id;
            team2.MatchId = match.Id.ToString();
            _context.Teams.Update(team1);
            _context.Teams.Update(team2);
            await _context.SaveChangesAsync();
            return gameRoom;
        }
        public async Task<bool> DropAllMatchRequests(Guid team1Id, Guid team2Id)
        {
            var matchRequests = await _context.Matches.Where(x => ((x.Team1Id == team1Id.ToString() || x.Team2Id == team1Id.ToString()) ||
                                        (x.Team1Id == team2Id.ToString() || x.Team2Id == team2Id.ToString())) &&
                                        x.IsAccepted == false).ToListAsync();
            var status = "";
            var statusSched = "";
            bool finalResult = false;
            foreach (Matches mat in matchRequests)
            {
                var schedulesM = await _context.Schedules.Where(x => x.MatchesId == mat.Id && x.Reserved == false).ToListAsync();
                foreach (Schedule sc in schedulesM)
                {
                    var statuscd = _context.Schedules.Remove(sc);
                    statusSched = statuscd.State.ToString();
                }
                if (statusSched.Equals("Deleted"))
                {
                    await _context.SaveChangesAsync();

                    var statusc = _context.Matches.Remove(mat);
                    status = statusc.State.ToString();

                    if (status.Equals("Deleted"))
                    {
                        await _context.SaveChangesAsync();
                        finalResult = true;
                    }
                    else { finalResult = false; }
                }
                else { finalResult = false; }
            }
            return finalResult;
        }
        public bool uploadImg(IFormFile img)
        {
            string ImgName;
            if (img == null)
            {
                return false;
            }
            else
            {
                ImgName = img.FileName;
                var fileExist = Path.Combine(environment.WebRootPath, "GameChat/ChatImgProfile", ImgName);
                if (!System.IO.File.Exists(fileExist))
                {
                    if (ImgName.EndsWith(".png") || ImgName.EndsWith(".jpg") || ImgName.EndsWith(".gif"))
                    {
                        var uploads = Path.Combine(environment.WebRootPath, "GameChat/ChatImgProfile");
                        var filePath = Path.Combine(uploads, ImgName);
                        img.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                return true;
            }
        }

        public async Task<bool> CreateMatch(Matches match)
        {
            var team1 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(match.Team1Id));
            var team2 = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(match.Team2Id));

            match.Name = team1.Name + " vs " + team2.Name;
            match.IsAccepted = false;
            if (match != null)
            {
                await _context.Matches.AddAsync(match);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ReserveSchedule(string SportFieldsId, DateTime startTime, DateTime endTime)
        {
            var scheduleExist = await _context.Schedules.FirstOrDefaultAsync(x => x.SportFieldId == Guid.Parse(SportFieldsId) && x.StartTime == startTime && x.EndTime == endTime);
            if (scheduleExist == null)
            {
                var schedule = new Schedule
                {
                    EndTime = endTime,
                    StartTime = startTime,
                    SportFieldId = Guid.Parse(SportFieldsId),
                    Reserved = false,
                };
                var status = await _context.AddAsync(schedule);
                if (status.State.ToString().Equals("Added"))
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<Matches> GetMatch(Guid id)
        {
            return await _context.Matches.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Team> GetTeam(string teamId)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(teamId));
        }
        public async Task<Schedule> GetMatchSchedule(string id)
        {
            return await _context.Schedules.FirstOrDefaultAsync(x => x.MatchesId == Guid.Parse(id));
        }
        public async Task<bool> FreeSchedule(string matchId)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.MatchesId == Guid.Parse(matchId));
            if (schedule == null) { return true; }
            var status = _context.Schedules.Remove(schedule);
            if (status.State.ToString().Equals("Deleted"))
            {
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
