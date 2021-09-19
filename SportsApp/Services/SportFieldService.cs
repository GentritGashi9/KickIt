using Microsoft.AspNetCore.Hosting;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using SportsApp.ViewModels;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class SportFieldService : ISportFieldService
    {
        private readonly ISportFieldRepository _sportFieldRepository;
        private readonly ISportFieldPicturesService _sportFieldPicturesService;
        private readonly ICategoryService _categoryService;
        private readonly IScheduleService _scheduleService;
        public SportFieldService(ISportFieldRepository sportFieldRepo, IWebHostEnvironment webHostEnvironment,
        ISportFieldPicturesService sportFieldPicturesService, ICategoryService categoryService,
        IScheduleService scheduleService)
        {
            _sportFieldRepository = sportFieldRepo;
            _sportFieldPicturesService = sportFieldPicturesService;
            _categoryService = categoryService;
            _scheduleService = scheduleService;
        }

        public async Task<List<SportFieldCategory>> Get()
        {
            return await _sportFieldRepository.Get();
        }
        public async Task<List<SportFieldCategory>> GetOwnerSportFields(string id)
        {
            return await _sportFieldRepository.GetOwnerSportFields(id);
        }
        public async Task<List<SportFieldCategory>> GetSpecificFieldsPending(string id)
        {
            return await _sportFieldRepository.GetSpecificFieldsPending(id);
        }
        public async Task<List<SportFieldCategory>> GetSpecificWithout(string id)
        {
            return await _sportFieldRepository.GetSpecificWithout(id);
        }
        public async Task<List<SportFieldPictures>> GetPictures(Guid id)
        {
            return await _sportFieldPicturesService.GetPicturesByField(id);
        }

        public async Task<SportField> Create(SportFieldViewModel model, string ownerId)
        {
            Guid id = Guid.NewGuid();
            Guid ownerID = Guid.Parse(ownerId);
            string workdays = "";
            foreach (int x in model.Workingdays)
            {
                workdays = workdays + "," + x.ToString();
            }

            SportField sportField = new SportField
            {
                Id = id,
                Name = model.Name,
                Address = model.Address,
                ContactNr = model.ContactNr,
                ApplicationUserId = ownerId,
                CategoryId = model.CategoryId,
                SportFieldGeoLocationLat = model.SportFieldGeoLocationLat,
                SportFieldGeoLocationLong = model.SportFieldGeoLocationLong,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                WorkDaysE = workdays
            };
            SportField result = await _sportFieldRepository.Create(sportField);
            string mainPicture = await _sportFieldPicturesService.UploadedFile(model, id);
            if (mainPicture == null)
            {
                mainPicture = "noImage.png";
            }
            var changedSportField = await changeMainPicture(sportField.Id, mainPicture);
            return changedSportField;
        }

        public async Task<SportField> changeMainPicture(Guid sportFieldId, string mainPicturePath)
        {
            return await _sportFieldRepository.changeMainPicture(sportFieldId, mainPicturePath);
        }

        public async Task<SportFieldCategory> Get(Guid id)
        {
            if (!id.ToString().Equals(""))
            {

                return await _sportFieldRepository.Get(id);
            }
            return null;
        }

        public async Task<SportField> Update(SportFieldViewModel model, Guid id)
        {
            string workdays = "";
            if (model.Workingdays != null || model.Workingdays.Count > 0)
            {
                foreach (int x in model.Workingdays)
                {
                    workdays = workdays + "," + x.ToString();
                }
            }
            SportField sf = new SportField
            {
                Id = id,
                Name = model.Name,
                Address = model.Address,
                ContactNr = model.ContactNr,
                CategoryId = model.CategoryId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                WorkDaysE = workdays
            };
            await _sportFieldPicturesService.UploadedFile(model, id);

            return await _sportFieldRepository.Update(sf);
        }
        public async Task<SportField> GetSpecificSportField(Guid id)
        {
            return await _sportFieldRepository.GetSpecificSportField(id);
        }
        public async Task<bool> Delete(Guid id)
        {
            return await _sportFieldPicturesService.DeletePictures(id) && await _sportFieldRepository.Delete(id);
        }

        public async Task<bool> DeletePicture(Guid id)
        {
            return await _sportFieldPicturesService.DeletePicture(id);
        }

        public Task<List<Category>> GetSportCategories()
        {
            return _categoryService.Get();
        }

        public void AddViewCount(Guid sportFieldId)
        {
            _sportFieldRepository.AddViewCount(sportFieldId);
        }

        public Task<List<SportFieldCategory>> GetTop4()
        {
            return _sportFieldRepository.GetTop4();
        }

        public async Task<List<Schedule>> GetSchedule(Guid id, string day)
        {
            List<Schedule> schedules = await _scheduleService.Get(id);
            SportFieldCategory sportField = await _sportFieldRepository.Get(id);

            TimeSpan timeSpan = sportField.EndTime - sportField.StartTime;
            List<Schedule> returnSchedule = new List<Schedule>();

            var today = DateTime.Now.Day;
            today = (DateTime.Parse(day)).Day - today;

            var weekday = ((int)DateTime.Parse(day).DayOfWeek-1) ;
            if(weekday == -1){
                weekday = 6;
            }
            var sportFieldsworkdays = sportField.WorkDaysE;

            DateTime sportFieldStartTime;
            var nextTime = sportField.StartTime;
            Schedule temp;
            bool exists = false;
 

            if (sportFieldsworkdays.Contains(weekday.ToString()))
            {
                for (var i = 0; i < timeSpan.TotalHours; i++)
                {
                    sportFieldStartTime = nextTime;
                    temp = new Schedule
                    {
                        StartTime = sportFieldStartTime.AddDays(today),
                        EndTime = sportFieldStartTime.AddHours(1).AddDays(today),
                    };

                    nextTime = sportFieldStartTime.AddHours(1);
                    if (today == 0)
                    {
                        if (temp.StartTime.Hour > DateTime.Now.ToLocalTime().Hour)
                        {
                            if (schedules.Count <= 0)
                            {
                                returnSchedule.Add(temp);
                            }
                            else
                            {
                                foreach (var schedule in schedules)
                                {
                                    if (schedule.StartTime.CompareTo(temp.StartTime) == 0 && schedule.EndTime.CompareTo(temp.EndTime) == 0)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (exists == false)
                                {
                                    returnSchedule.Add(temp);
                                }
                                exists = false;
                            }
                        }
                    }
                    else
                    {
                        if (schedules.Count <= 0)
                        {
                            returnSchedule.Add(temp);
                        }
                        else
                        {
                            foreach (var schedule in schedules)
                            {
                                if (schedule.StartTime.CompareTo(temp.StartTime) == 0 && schedule.EndTime.CompareTo(temp.EndTime) == 0)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (exists == false)
                            {
                                returnSchedule.Add(temp);
                            }
                            exists = false;
                        }
                    }
                }
            }
            return returnSchedule;
        }

        public Task<Schedule> CreateSchedule(DateTime startTime, DateTime endTime, Guid sportFieldId)
        {
            Schedule schedule = new Schedule
            {
                StartTime = startTime,
                EndTime = endTime,
                SportFieldId = sportFieldId
            };

            return _scheduleService.Create(schedule);
        }

        public Task<List<SportFieldCategory>> GetSportfieldFromCategory(Guid categoryId)
        {
            return _sportFieldRepository.GetSportfieldFromCategory(categoryId);
        }

        public Task<List<ScheduleViewModel>> GetSchedulesForFieldOwner(Guid sportFieldId)
        {
            return _scheduleService.GetSchedulesForFieldOwner(sportFieldId);
        }
    }
}

