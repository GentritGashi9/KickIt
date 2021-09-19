using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Team> Create(Team team)
        {
            Team x = await _context.Teams.FirstOrDefaultAsync(x => ((x.Name == team.Name) && (x.CategoryId == team.CategoryId)));
            if (x != null)
            {
                return new Team { Name = "Err42c0k7:This team name already exists in this sport category! Please chose another name." };
            }
            else
            {
                if (team != null)
                {
                    await _context.Teams.AddAsync(team);
                    ApplicationUser user = await _context.Users.Where(x => x.Id == team.TeamLeaderId.ToString()).FirstOrDefaultAsync();
                    user.TeamId = team.Id;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                return team;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null)
            {
                return false;
            }
            else
            {
                ApplicationUser player =  await _context.Users.FirstOrDefaultAsync(x => x.TeamId == id);
                player.TeamId = null;
                _context.Users.Update(player);

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<TeamViewModel>> GetMyJoinRequests(Guid playerId)
        {
            List<TeamRequests> teamRequests = await _context.TeamsRequests.Where(x => x.PlayerId == playerId.ToString()).ToListAsync();
            var tRequests = new List<TeamViewModel>();
            foreach (TeamRequests up in teamRequests)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == up.TeamId);
                string categoryName = (await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId)).Name;
                TeamViewModel teamModelFiltred = new TeamViewModel
                {
                    TeamLeaderName = await GetTeamLeaderUserName(team.Id),
                    Name = team.Name,
                    CityName = team.City.ToString(),
                    CategoryId = team.CategoryId,
                    CategoryName = categoryName.ToLower(),
                    TeamLeaderId = team.TeamLeaderId.ToString(),
                    Id = team.Id
                };
                tRequests.Add(teamModelFiltred);
            }
            return tRequests;
        }
        public async Task<List<TeamViewModel>> GetMyJoinRequestFiltred(Guid playerId,string category,string city)
        {
            Cities cityNumber = (Cities)Enum.Parse(typeof(Cities), city);
            List<TeamRequests> teamRequests = await _context.TeamsRequests.Where(x => x.PlayerId == playerId.ToString() &&
                                                                                      x.Team.Category.Name == category &&
                                                                                       x.Team.City ==cityNumber).ToListAsync();
            var tRequests = new List<TeamViewModel>();
            foreach (TeamRequests up in teamRequests)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == up.TeamId);
                string categoryName = (await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId)).Name;
                TeamViewModel teamModelFiltred = new TeamViewModel
                {
                    TeamLeaderName = await GetTeamLeaderUserName(team.Id),
                    Name = team.Name,
                    CityName = team.City.ToString(),
                    CategoryId = team.CategoryId,
                    CategoryName = categoryName.ToLower(),
                    TeamLeaderId = team.TeamLeaderId.ToString(),
                    Id = team.Id
                };
                tRequests.Add(teamModelFiltred);
            }
            return tRequests;
        }
        public async Task<List<ApplicationUser>> GetMyTeamRequests(Guid teamId)
        {
            List<TeamRequests> teamRequests = await _context.TeamsRequests.Where(x=>x.TeamId==teamId).ToListAsync();
            var plRequests = new List<ApplicationUser>();
            foreach(TeamRequests up in teamRequests)
            {
                var player = await _context.Users.FirstOrDefaultAsync(x => x.Id == up.PlayerId);
                plRequests.Add(player);
            }
            return plRequests;

        }
        public async Task<Team> Get(Guid id)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Team> GetCurrentUserTeam(Guid? id)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> IsTeamFull(Guid id)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            Category categoryteam = await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId);
            int result = await GetNumberOfPlayers(id);

            if (result >= categoryteam.MaxCapacity ) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<TeamViewModel>> GetAllFiltredTeams(Guid? currentUserTeamId)
        {
            List<Team> teamsFiltred = new List<Team>();
            if (currentUserTeamId == null)
            {
                teamsFiltred = await _context.Teams.ToListAsync();
            }
            else
            {
                teamsFiltred = await _context.Teams.Where(x => x.Id != currentUserTeamId).ToListAsync();
            }            

            List<TeamViewModel> teamsModelFiltred = new List<TeamViewModel>();
            foreach (Team team in teamsFiltred)
            {
                int numberOfPlayers = await GetNumberOfPlayers(team.Id);
                Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId);
                string categoryName = category.Name;
                int maxCategory = category.MaxCapacity;
                TeamViewModel teamModelFiltred = new TeamViewModel
                {
                    TeamLeaderName = await GetTeamLeaderUserName(team.Id),
                    Name = team.Name,
                    CityName = team.City.ToString(),
                    CategoryId = team.CategoryId,
                    CategoryName = categoryName.ToLower(),
                    TeamLeaderId = team.TeamLeaderId.ToString(),
                    Id = team.Id,
                    isPrivate = team.isPrivate,
                    NumberOfPlayers = numberOfPlayers,
                    MaxPlayers = maxCategory
                };
                teamsModelFiltred.Add(teamModelFiltred);
            }
            return teamsModelFiltred;
        }

        public async Task<List<TeamViewModel>> GetFiltredTeams(string category, string city, Guid? currentUserTeamId)
        {
            Cities cityNumber = (Cities)Enum.Parse(typeof(Cities), city);

            List<Team> teamsFiltred = new List<Team>();
            if (currentUserTeamId == null)
            {
                teamsFiltred = await _context.Teams.Where(x => (x.Category.Name == category) && (x.City == cityNumber)).ToListAsync();
            }
            else
            {
                teamsFiltred = await _context.Teams.Where(x => (x.Category.Name == category) && (x.City == cityNumber) && (x.Id != currentUserTeamId)).ToListAsync();
            }
            
            List<TeamViewModel> teamsModelFiltred = new List<TeamViewModel>();
            foreach (Team team in teamsFiltred)
            {
                int numberOfPlayers = await GetNumberOfPlayers(team.Id);
                int maxCategory = (await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId)).MaxCapacity;
                TeamViewModel teamModelFiltred = new TeamViewModel
                {
                    TeamLeaderName = await GetTeamLeaderUserName(team.Id),
                    Name = team.Name,
                    CityName = team.City.ToString(),
                    CategoryId = team.CategoryId,
                    CategoryName = category.ToLower(),
                    TeamLeaderId = team.TeamLeaderId.ToString(),
                    Id = team.Id,
                    isPrivate = team.isPrivate,
                    NumberOfPlayers = numberOfPlayers,
                    MaxPlayers = maxCategory
                };
                teamsModelFiltred.Add(teamModelFiltred);
            }
            return teamsModelFiltred;
        }

        public async Task<Team> Update(Team teamChanges)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamChanges.Id);
            if (team != null)
            {
                team.Name = teamChanges.Name;
                team.City = teamChanges.City;
            }
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<string> GetTeamLeaderUserName(Guid id)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if(team != null)
            {
                Guid teamLeaderId = team.TeamLeaderId;
                ApplicationUser teamLeader = await _context.Users.FirstOrDefaultAsync(x => x.Id == teamLeaderId.ToString());
                return teamLeader.UserName;
            }
            return null;
        }

        public async Task<bool> PlayerHasATeam(Guid playerId)
        {
            ApplicationUser player = await _context.Users.FirstOrDefaultAsync(e => e.Id.Equals(playerId.ToString()));
            
            if(player.TeamId == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<TeamViewModel>> Get()
        {
            List<TeamViewModel> teams = await (from team in _context.Teams
                                    join user in _context.Users
                                    on team.TeamLeaderId.ToString() equals user.Id into users
                                    from leader in users.DefaultIfEmpty()
                                    join category in _context.Categories
                                    on team.CategoryId equals category.Id into categories
                                    from category in categories.DefaultIfEmpty()
                                    select new TeamViewModel
                                    {
                                        TeamLeaderName = leader == null ? "No Team Leader" : leader.UserName,
                                        Name = team.Name,
                                        City = team.City,
                                        CityName = team.City.ToString(),
                                        CategoryId = team.CategoryId,
                                        CategoryName = category.Name,
                                        TeamLeaderId = team.TeamLeaderId.ToString(),
                                        Id = team.Id
                                    }).ToListAsync();
            return teams;
        }
        public async Task<Dictionary<bool,string>> Request(Guid teamId,Guid playerId)
        {
            ApplicationUser player = await _context.Users.FirstOrDefaultAsync(x => x.Id == playerId.ToString());
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            int requestCount = (await _context.TeamsRequests.Where(x => x.PlayerId == playerId.ToString()).ToListAsync()).Count;
            TeamRequests existRequest = await _context.TeamsRequests.FirstOrDefaultAsync(x => ((x.TeamId == teamId) && (x.PlayerId == playerId.ToString())));
            int s = await GetNumberOfPlayersRequests(teamId);
            int noPlayers = await GetNumberOfPlayers(teamId);
            Category teamCategory = await GetTeamCategory(teamId);
            Dictionary<bool, string> msg = new Dictionary<bool, string>();
            if (await PlayerHasATeam(playerId))
            {
                msg.Add(false, "You already have a team!");
                return msg;
            }
            else if (existRequest != null)
            {
                msg.Add(false, "You can't request to join this team because you already requested this team!");
                return msg;
            }
            else if (s >= (teamCategory.MaxCapacity + (teamCategory.MaxCapacity / 2)))
            {
                msg.Add(false, "You can't request to join this team because there are already too many requests!");
                return msg;
            }
            else if (noPlayers >= teamCategory.MaxCapacity)
            {
                msg.Add(false, "You can't request to join this team because this team is full!!");
                return msg;
            }
            else if (requestCount >= 10)
            {
                msg.Add(false, "You can't request to join this team because you already requested to join in more than 10 teams!");
                return msg;
            }
            else
            {
                var request = new TeamRequests() {
                    Id = Guid.NewGuid(),
                    Player = player,
                    PlayerId = playerId.ToString(),
                    Team = team,
                    TeamId = teamId
                };
                await _context.TeamsRequests.AddAsync(request);
                await _context.SaveChangesAsync();
                msg.Add(true, "Your request was sent successfully, you have to wait for Team's leader approval!");
                return msg;
            }
        }
        public async Task<bool> ApproveRequest(Guid teamId,Guid playerId)
        {
            List<TeamRequests> allPlayerRequests = await _context.TeamsRequests.Where(x => x.PlayerId == playerId.ToString()).ToListAsync();
            TeamRequests currentRequest = await _context.TeamsRequests.FirstOrDefaultAsync(x => ((x.PlayerId == playerId.ToString()) && (x.TeamId == teamId)));
            if(currentRequest == null) { return false; }
            foreach (TeamRequests a in allPlayerRequests)
            {
                 _context.TeamsRequests.Remove(a);
            }
            await _context.SaveChangesAsync();

            string resultAsString = await Join(teamId, playerId);
            bool result = bool.Parse(resultAsString.Split(":")[0]);
            if (result){ return result; }
            else { return result; }
        }
        public async Task<bool> RefuseRequest(Guid teamId, Guid playerId)
        {
            TeamRequests playerRequest = await _context.TeamsRequests.FirstOrDefaultAsync(x => ((x.PlayerId == playerId.ToString()) && (x.TeamId == teamId)));
            if (playerRequest == null) { return false; }

            var status =_context.TeamsRequests.Remove(playerRequest);
            if (status.State.ToString().Equals("Deleted")) {
                await _context.SaveChangesAsync();
                return true;
            }
            else { return false; }

        }
        public async Task<bool> DropAllPlayerRequests(string playerId)
        {
            List<TeamRequests> allPlayerRequests = await _context.TeamsRequests.Where(x => x.PlayerId == playerId.ToString()).ToListAsync();
            if (allPlayerRequests == null) { return true; }
            if (allPlayerRequests.Count == 0) { return true; }
            var status = "";
            foreach (TeamRequests a in allPlayerRequests)
            {
               var statuc = _context.TeamsRequests.Remove(a);
                status = statuc.State.ToString();
            }
            if (status.Equals("Deleted"))
            {
                await _context.SaveChangesAsync();
                return true;
            }
            else { return false; }

        }
        public async Task<string> DropOneRequest(string playerId,string teamId)
        {
            TeamRequests specificRequests = await _context.TeamsRequests.FirstOrDefaultAsync(x => ((x.PlayerId == playerId.ToString()) && (x.TeamId == Guid.Parse(teamId))));
            if (specificRequests == null) { return "False:This request was already refused by the team leader!"; }

            var status = _context.TeamsRequests.Remove(specificRequests);
            if (status.State.ToString().Equals("Deleted"))
            {
                await _context.SaveChangesAsync();
                return "True:This request was cancelled successfully";
            }
            else { return "False:This request wasn't cancelled! Something failed."; }

        }
        public async Task<string> Join(Guid teamId, Guid playerId)
        {
            Dictionary<bool, string> result = new Dictionary<bool, string>();
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            ApplicationUser player = await _context.Users.FirstOrDefaultAsync(x => x.Id == playerId.ToString());
            int s = await GetNumberOfPlayers(teamId);
            Category teamCategory = await GetTeamCategory(teamId);
            if (await PlayerHasATeam(playerId))
            {
                return "False:You already have a team!";
            }
            else if (s >= teamCategory.MaxCapacity) {
                return "False:This team is full!";
            }
            else if (team == null)
            {
                return "False:Team doesn't exist anymore!";
            }
            else
            {
                var requestResult = await DropAllPlayerRequests(playerId.ToString());
                var notificationResult = await DropAllNotifications(playerId.ToString());
                if (requestResult && notificationResult)
                {
                    player.TeamId = teamId;
                    _context.Users.Update(player);
                    await _context.SaveChangesAsync();
                    return "True:You have joined " + team.Name + "!";
                }
                else
                {
                    return requestResult ? (notificationResult ? "Impossible" : "False:Failed to remove player notifications!") : "Flase:Failed to delete players requests!";
                }
            }
 
        }
        
        public async Task<string> JoinInvitation(Guid teamId, Guid playerId)
        {
            ApplicationUser player = await _context.Users.FirstOrDefaultAsync(x => x.Id == playerId.ToString());
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            
            if (await PlayerHasATeam(playerId))
            {
                return "False:You already have a team!";
            }else if (team == null)
            {
                return "False:Team doesn't exist anymore!";
            }
            else
            {
                player.TeamId = teamId;
                _context.Users.Update(player);
                await _context.SaveChangesAsync();
                return "True:You joined "+(team.Name)+"!";
            }
        }
        
        public async Task<bool> Leave(Guid teamId, Guid playerId)
        {
            if(!await PlayerIsLeader(teamId, playerId))
            {
                await DropAllNotifications(playerId.ToString());
                ApplicationUser player = await _context.Users.FirstOrDefaultAsync(x => x.Id == playerId.ToString());
                player.TeamId = null;
                _context.Users.Update(player);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;            
        }

        public async Task<bool> PlayerIsLeader(Guid teamId, Guid playerId)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            if(team != null && team.TeamLeaderId != playerId)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckAlreadyRequested(Guid teamId,string userId)
        {
            TeamRequests teamReq = await _context.TeamsRequests.FirstOrDefaultAsync(x => ((x.TeamId == teamId) && (x.PlayerId == userId)));
            if (teamReq == null)
            {
                return false;
            }
            return true;
        }
        public async Task<Dictionary<ApplicationUser,bool>> GetTeamPlayers(Guid teamId)
        {
            Dictionary<ApplicationUser, bool> allPlayers = new Dictionary<ApplicationUser, bool>();

            List<ApplicationUser> players = await _context.Users.Where(x => x.TeamId == teamId).ToListAsync();
            foreach (ApplicationUser p in players)
            {
                bool result = await PlayerIsLeader(teamId, Guid.Parse(p.Id));
                allPlayers.Add(p, result);
            }
            return allPlayers;
        }

        public async Task<int> GetNumberOfPlayers(Guid teamId)
        {
            List<ApplicationUser> players = await _context.Users.Where(x => x.TeamId == teamId).ToListAsync();
            return players.Count;
        }
        public async Task<int> GetNumberOfPlayersRequests (Guid teamId)
        {
            List<TeamRequests> players = await _context.TeamsRequests.Where(x => x.TeamId == teamId).ToListAsync();
            return players.Count;
        }
        public async Task<Category> GetTeamCategory(Guid teamId)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            Category teamCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == team.CategoryId);
            return teamCategory;
        }
        public async Task<bool> ChangeAccess(Guid teamId)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            if(team != null)
            {
                if (await IsTeamPrivate(teamId))
                {
                    team.isPrivate = false;
                }
                else if(!(await IsTeamPrivate(teamId)))
                {
                    team.isPrivate = true;
                }
                _context.Teams.Update(team);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> IsTeamPrivate(Guid teamId)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            if (team != null)
            {
                if (team.isPrivate == true)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<string>> UsernameAutoComplete(string usernamePrefix,string currentUser)
        {
            
            List<string> usernames = new List<string>();

            List<ApplicationUser> users = await _context.Users.Where(x => x.UserName.StartsWith(usernamePrefix) && x.UserName != currentUser).ToListAsync();
            foreach(ApplicationUser user in users)
            {
                usernames.Add(user.UserName);
            }
            return usernames;
        }
        public async Task<bool> DropAllNotifications(string userId)
        {
            var notifications =  await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            if (notifications == null) { return true; }
            if (notifications.Count == 0) { return true; }
            bool check = false;
            foreach (Notification x in notifications)
            {
                var status = _context.Notifications.Remove(x);
                if (status.State.ToString().Equals("Deleted")) { check = true; }
            }
            if (check) {  await _context.SaveChangesAsync(); return true; }
            return false;
        }
    }
}
