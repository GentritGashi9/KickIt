using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsApp.Models;
using SportsApp.ViewModels;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportfieldId);
        Task<List<Schedule>> Get(Guid id);
        Task<Schedule> Create(Schedule schedule);
        
    }
}