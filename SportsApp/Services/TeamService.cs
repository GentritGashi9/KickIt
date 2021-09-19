using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICategoryService _categoryService;
     
        public TeamService(ITeamRepository teamRepository, ICategoryService categoryService,INotificationRepository notificationRepository)
        {
            _teamRepository = teamRepository;
            _categoryService = categoryService;
            _notificationRepository = notificationRepository;
        }

        public async Task<Team> Create(TeamViewModel teamViewModel)
        {
            teamViewModel.Id = Guid.NewGuid();
            Team team = new Team
            {
                Id = teamViewModel.Id,
                Name = teamViewModel.Name,
                City = teamViewModel.City,
                CategoryId = teamViewModel.CategoryId,
                TeamLeaderId = Guid.Parse(teamViewModel.TeamLeaderId),
                isPrivate = teamViewModel.isPrivate
            };
            Team result = await _teamRepository.Create(team);
            return result;
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _teamRepository.Delete(id);
        }

        public async Task<List<TeamViewModel>> GetAllFiltredTeams(Guid? currentUserTeamId)
        {
            return await _teamRepository.GetAllFiltredTeams(currentUserTeamId);
        }

        public async Task<List<TeamViewModel>> GetFiltredTeams(string category, string city, Guid? currentUserTeamId)
        {
            return await _teamRepository.GetFiltredTeams(category, city, currentUserTeamId);
        }

        public async Task<List<TeamViewModel>> Get()
        {
            return await _teamRepository.Get();
        }

        public async Task<Team> Get(Guid id)
        {
            if (!id.ToString().Equals(""))
            {
                return await _teamRepository.Get(id);
            }
            return null;
        }
        public async Task<List<ApplicationUser>> GetMyTeamRequests(Guid teamId)
        {
            if (!teamId.ToString().Equals(""))
            {
                return await _teamRepository.GetMyTeamRequests(teamId);
            }
            return null;
        }
        public async Task<List<TeamViewModel>> GetMyJoinRequests(Guid playerId)
        {
            if (!playerId.ToString().Equals(""))
            {
                return await _teamRepository.GetMyJoinRequests(playerId);
            }
            return null;
        }
        public async Task<List<TeamViewModel>> GetMyJoinRequestFiltred(Guid playerId, string category, string city)
        {
            if (!playerId.ToString().Equals("") && !category.Equals("") && !city.Equals(""))
            {
                return await _teamRepository.GetMyJoinRequestFiltred(playerId,category,city);
            }
            return null;
        }
        public async Task<Team> GetCurrentUserTeam(Guid? id)
        {
            if (!id.ToString().Equals(""))
            {
                return await _teamRepository.GetCurrentUserTeam(id);
            }
            return null;
        }

        public async Task<Team> Update(TeamViewModel teamViewModel, Guid Id)
        {
            Team team = new Team
            {
                Id = Id,
                Name = teamViewModel.Name,
                City = teamViewModel.City
            };
            return await _teamRepository.Update(team);
        }

        public Task<List<Category>> GetSportCategories()
        {
            return _categoryService.Get();
        }

        public async Task<string> GetSportCategoryName(Guid categoryId)
        {
            return await _categoryService.GetName(categoryId);
        }

        public async Task<string> GetTeamLeaderUserName(Guid id)
        {
            return await _teamRepository.GetTeamLeaderUserName(id);
        }

        public async Task<bool> PlayerHasATeam(Guid playerId)
        {
            return await _teamRepository.PlayerHasATeam(playerId);
        }
        public async Task<bool> ApproveRequest(Guid teamId, Guid playerId)
        {
            return await _teamRepository.ApproveRequest(teamId,playerId);
        }
        public async Task<bool> RefuseRequest(Guid teamId, Guid playerId)
        {
            return await _teamRepository.RefuseRequest(teamId,playerId);
        }
        public async Task<bool> DropAllPlayerRequests(string playerId)
        {
            return await _teamRepository.DropAllPlayerRequests( playerId);
        }
        public async Task<string> DropOneRequest(string playerId, string teamId)
        {
            return await _teamRepository.DropOneRequest(playerId,teamId);

        }
        public async Task<Dictionary<bool, string>> Request(Guid teamId, Guid playerId)
        {
            return await _teamRepository.Request(teamId, playerId);
        }
        public async Task<string> Join(Guid teamId, Guid playerId)
        {
            return await _teamRepository.Join(teamId, playerId);
        }
        public async Task<string> JoinInvitation(Guid teamId, Guid playerId)
        {
            return await _teamRepository.Join(teamId, playerId);
        }

        public async Task<bool> Leave(Guid teamId, Guid playerId)
        {
            return await _teamRepository.Leave(teamId, playerId);
        }

        public async Task<bool> PlayerIsLeader(Guid teamId, Guid playerId)
        {
            return await _teamRepository.PlayerIsLeader(teamId, playerId);
        }
        public async Task<bool> CheckAlreadyRequested(Guid teamId, string userId)
        {
            return await _teamRepository.CheckAlreadyRequested(teamId, userId);
        }
        public async Task<Dictionary<ApplicationUser, bool>> GetTeamPlayers(Guid teamId)
        {
            return await _teamRepository.GetTeamPlayers(teamId);
        }
        public async Task<string> InviteForJoin(string teamName, string username)
        {
            return await _notificationRepository.InviteForJoin(teamName,username);
        }

        public async Task<bool> JoinNotificationIsRead(string notificationId) 
        {
            return await _notificationRepository.JoinNotificationIsRead(notificationId);
        }
        public async Task<string> NotificationTypeCheck(string notificationId)
        {
            return await _notificationRepository.NotificationTypeCheck(notificationId);
        }
        public async Task<List<string>> UsernameAutoComplete(string usernamePrefix,string currentUser)
        {
            return await _teamRepository.UsernameAutoComplete(usernamePrefix,currentUser);
        }

        public Task<bool> DeleteNotification(Guid Id)
        {
            return _notificationRepository.Delete(Id);
        }
        public Task<bool> DropAllNotifications(string userId)
        {
            return _teamRepository.DropAllNotifications(userId);
        }
        public async Task<bool> IsTeamFull(Guid teamId)
        {
            return await _teamRepository.IsTeamFull(teamId);
        }
        public async Task<int> GetNumberOfPlayers(Guid teamId)
        {
            return await _teamRepository.GetNumberOfPlayers(teamId);
        }
        public async Task<bool> ChangeAccess(Guid teamId)
        {
            return await _teamRepository.ChangeAccess(teamId);
        }
        public async Task<bool> IsTeamPrivate(Guid teamId)
        {
            return await _teamRepository.IsTeamPrivate(teamId);
        }
    }
}
