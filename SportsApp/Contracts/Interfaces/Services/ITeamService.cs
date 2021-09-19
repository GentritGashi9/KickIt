using Microsoft.AspNetCore.Mvc;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface ITeamService
    {
        Task<Team> Create(TeamViewModel teamViewModel);
        Task<List<TeamViewModel>> Get();
        Task<List<TeamViewModel>> GetAllFiltredTeams(Guid? currentUserTeamId);
        Task<List<TeamViewModel>> GetFiltredTeams(string category, string city, Guid? currentUserTeamId);
        Task<Team> Get(Guid id);
        Task<Team> GetCurrentUserTeam(Guid? id);
        Task<List<ApplicationUser>> GetMyTeamRequests(Guid teamId);
        Task<List<TeamViewModel>> GetMyJoinRequests(Guid playerId);
        Task<List<TeamViewModel>> GetMyJoinRequestFiltred(Guid playerId, string category, string city);
        Task<bool> Delete(Guid id);
        Task<Team> Update(TeamViewModel teamViewModel, Guid Id);
        Task<string> GetTeamLeaderUserName(Guid id);
        Task<bool> PlayerHasATeam(Guid playerId);
        Task<Dictionary<bool, string>> Request(Guid teamId, Guid playerId);
        Task<bool> ApproveRequest(Guid teamId, Guid playerId);
        Task<bool> RefuseRequest(Guid teamId, Guid playerId);
        Task<bool> DropAllPlayerRequests(string playerId);
        Task<string> DropOneRequest(string playerId,string teamId);
        Task<bool> CheckAlreadyRequested(Guid teamId, string userId);
        Task<string> Join(Guid teamId, Guid playerId);
        Task<string> JoinInvitation(Guid teamId, Guid playerId);
        Task<bool> Leave(Guid teamId, Guid playerId);
        Task<bool> PlayerIsLeader(Guid teamId, Guid playerId);
        Task<bool> IsTeamFull(Guid id);
        Task<Dictionary<ApplicationUser, bool>> GetTeamPlayers(Guid teamId);
        Task<List<Category>> GetSportCategories();
        Task<string> GetSportCategoryName(Guid categoryId);
        Task<int> GetNumberOfPlayers(Guid teamId);
        Task<bool> ChangeAccess(Guid teamId);
        Task<bool> IsTeamPrivate(Guid teamId);
        Task<string> InviteForJoin(string teamName, string username);
        Task<bool> JoinNotificationIsRead(string notificationId);
        Task<string> NotificationTypeCheck(string notificationId);
        Task<List<string>> UsernameAutoComplete(string usernamePrefix,string currentUser);
        Task<bool> DeleteNotification(Guid Id);
        Task<bool> DropAllNotifications(string userId);
    }
}
