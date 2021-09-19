using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;

namespace SportsApp.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Schedule> Create(Schedule schedule)
        {
            if (schedule != null)
            {
                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();
                return schedule;
            }
            return null;
        }

        public async Task<List<Schedule>> Get1(Guid sportFieldId)
        {
            return await _context.Schedules.Where(x => x.SportFieldId == sportFieldId).ToListAsync(); ;
        }
        public async Task<List<Schedule>> Get(Guid sportFieldId)
        {
            return await _context.Schedules.Where(x => x.SportFieldId == sportFieldId).ToListAsync();
        }

        
        public async Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportfieldId)
        {
            List<ScheduleViewModel> schedules = await(from s in _context.Schedules
                                                         join m in _context.Matches
                                                         on s.MatchesId equals m.Id
                                                         into MatchSchedule
                                                         from t in MatchSchedule
                                                         where s.SportFieldId == sportfieldId 
                                                         select new ScheduleViewModel
                                                         {
                                                             MatchId = t.Id,
                                                             MatchName = t.Name,
                                                             StartTime = s.StartTime,
                                                             EndTime = s.EndTime
                                                         }).ToListAsync();
                  
            return schedules;
        }
    }
}