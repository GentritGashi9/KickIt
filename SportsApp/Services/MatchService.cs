using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly INotificationRepository _notificationRepository;

        public MatchService(IMatchRepository matchRepository, INotificationRepository notificationRepository)
        {
            _matchRepository = matchRepository;
            _notificationRepository = notificationRepository;
        }
        public async Task<string> CurrentUserTeamId(ApplicationUser user)
        {
            return await _matchRepository.CurrentUserTeamId(user);
        }
        public async Task<GameRoom> GetGameRoom(string id)
        {
            return await _matchRepository.GetGameRoom(id);
        }
        public async Task<List<Team>> GetGameRoomTeams(string id)
        {
            return await _matchRepository.GetGameRoomTeams(id);
        }
        public async Task<GameRoom> AcceptForMatch(string matchId/*,IFormFile img*/)
        {
            return await _matchRepository.AcceptForMatch(matchId/*,img*/);
        }
        public async Task<bool> RefuseForMatch(string matchId)
        {
            return await _matchRepository.RefuseForMatch(matchId);
        }
        public async Task<List<Matches>> AcceptMatch(string teamId)
        {
            return await _matchRepository.AcceptMatch(teamId);
        }

        public async Task<string> AskForMatch(string team1Id, string team2Id, string sportFieldsId,DateTime startTime,DateTime endTime)
        {
            return await _matchRepository.AskForMatch(team1Id, team2Id, sportFieldsId, startTime, endTime);
        }

        public async Task<bool> NotificationAskForMatch(string teamId,string teamidsender)
        {
            return await _notificationRepository.InviteForMatch(teamId, teamidsender);
        }

        public async Task<List<Team>> Match(ApplicationUser user)
        {
            return await _matchRepository.Match(user);
        }

        public Task<bool> CreateMatch(string team1, string team2, int scheduleId)
        {
            Matches match = new Matches{
                Team1Id = team1,
                Team2Id = team2,
                //ScheduleId = scheduleId
            };

            return _matchRepository.CreateMatch(match);
        }

        public async Task<string> MatchExists(string team1Id, string team2Id,string sportFieldsId)
        {
            return await _matchRepository.MatchCheck(team1Id,team2Id,sportFieldsId);
        }
        public async Task<bool> ReserveSchedule(string sportFieldsId,DateTime startTime,DateTime endTime)
        {
            return await _matchRepository.ReserveSchedule(sportFieldsId, startTime, endTime);
        }
        public async Task<Matches> GetMatch(Guid id)
        {
            return await _matchRepository.GetMatch(id);
        }
        public async Task<Team> GetTeam(string teamId)
        {
            return await _matchRepository.GetTeam(teamId);
        }
        public async Task<Schedule> GetMatchSchedule(string id)
        {
            return await _matchRepository.GetMatchSchedule(id);
        }
        public async Task<bool> FreeSchedule(string matchId)
        {
            return await _matchRepository.FreeSchedule(matchId);
        }
    }
}

