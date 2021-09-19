using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsApp.Models;
using SportsApp.ViewModels;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> Get(Guid id);
        Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportfieldId);
        Task<Schedule> Create(Schedule schedule);
    }
}