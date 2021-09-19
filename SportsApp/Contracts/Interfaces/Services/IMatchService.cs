using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IMatchService
    {
        Task<bool> CreateMatch(string team1 , string team2,int scheduleId);
        public Task<string> MatchExists(string team1Id, string team2Id,string sportFieldsId);
        public Task<string> CurrentUserTeamId(ApplicationUser user);
        public Task<GameRoom> GetGameRoom(string id);
        public Task<List<Team>> GetGameRoomTeams(string id);
        public Task<List<Team>> Match(ApplicationUser user);
        public Task<bool> RefuseForMatch(string matchId);
        public Task<string> AskForMatch(string Team1Id, string Team2Id, string SportFieldsId, DateTime startTime, DateTime endTime);
        public Task<bool> NotificationAskForMatch(string teamId,string teamidsender);
        public Task<List<Matches>> AcceptMatch(string teamId);
        public Task<GameRoom> AcceptForMatch(string matchId);
        public Task<bool> ReserveSchedule(string sportFieldsId, DateTime startTime, DateTime endTime);
        public Task<Matches> GetMatch(Guid id);
        public Task<Team> GetTeam(string teamId);
        public Task<Schedule> GetMatchSchedule(string id);
        public Task<bool> FreeSchedule(string matchId);

    }
}
