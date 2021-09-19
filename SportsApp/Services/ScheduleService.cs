using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using SportsApp.ViewModels;

namespace SportsApp.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public Task<Schedule> Create(Schedule schedule)
        {
            return _scheduleRepository.Create(schedule);
        }
        public async Task<List<Schedule>> Get(Guid id)
        {
            return await  _scheduleRepository.Get(id);
        }
        public Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportfieldId)
        {
            return _scheduleRepository.GetSchedulesForFieldOwner(sportfieldId);
        }
    }
}