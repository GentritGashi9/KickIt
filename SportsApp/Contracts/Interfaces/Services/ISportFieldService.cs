using SportsApp.Models;
using SportsApp.ViewModels;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface ISportFieldService
    {
        public Task<List<SportFieldCategory>> Get();
        public Task<List<SportFieldCategory>> GetTop4();
        public Task<List<SportFieldCategory>> GetSportfieldFromCategory(Guid categoryId);
        Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportFieldId);
        public Task<List<SportFieldCategory>> GetOwnerSportFields(string id);
        public Task<List<SportFieldCategory>> GetSpecificWithout(string id);
        public Task<List<SportFieldCategory>> GetSpecificFieldsPending(string id);
        public Task<List<Category>> GetSportCategories();
        Task<List<Schedule>> GetSchedule(Guid id,string day);
        Task<Schedule> CreateSchedule(DateTime startTime,DateTime endTime,Guid sportFieldId);
        public Task<SportField> Create(SportFieldViewModel model, string id);
        public Task<SportFieldCategory> Get(Guid id);
        public Task<SportField> GetSpecificSportField(Guid id);
        public Task<SportField> Update(SportFieldViewModel model, Guid id);
        public Task<bool> Delete(Guid id);
        public Task<List<SportFieldPictures>> GetPictures(Guid id);
        public Task<bool> DeletePicture(Guid id);
        public void AddViewCount(Guid sportFieldId);
        public Task<SportField> changeMainPicture(Guid sportFieldId, string mainPicturePath);
    }
}
