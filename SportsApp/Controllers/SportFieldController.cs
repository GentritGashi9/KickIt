using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{
    [Authorize]
    public class SportFieldController : Controller
    {
        private readonly ISportFieldService _sportFieldService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SportFieldController(ISportFieldService sportFieldService, UserManager<ApplicationUser> userManager)
        {
            _sportFieldService = sportFieldService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> AllSportFieldsList()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;
                ViewData["Id"] = userId;
            }
            return View();
        }

        // GET: SportFields
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(new { data = await _sportFieldService.Get() });
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> GetSpecificFields(string id)
        {
            return Json(new { data = await _sportFieldService.GetOwnerSportFields(id) });
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> GetSpecificFieldsPending(string id)
        {
            return Json(new { data = await _sportFieldService.GetSpecificFieldsPending(id) });
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> GetSpecificFieldsWithout(string id)
        {
            return Json(new { data = await _sportFieldService.GetSpecificWithout(id) });
        }

        // GET: SportField/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string currentLoginInUserId = _userManager.GetUserId(User);
            ViewData["userUid"] = currentLoginInUserId;

            List<SportFieldPictures> sportFieldPictures = await _sportFieldService.GetPictures(id);
            ViewData["sportFieldPictures"] = sportFieldPictures;
            SportFieldCategory sportField = await Get(id);                                                          //fix this
            List<Schedule> schedules = await _sportFieldService.GetSchedule(sportField.SportFieldId, DateTime.Now.ToString());

            if (sportField.ApplicationUserId.Equals(currentLoginInUserId))
            {
                List<ScheduleViewModel> scheduledMatches = await _sportFieldService.GetSchedulesForFieldOwner(id);
                ViewData["AppointedSchedules"] = scheduledMatches;
            }
            ViewData["Schedules"] = schedules;
            if (sportField == null)
            {
                return NotFound();
            }
            _sportFieldService.AddViewCount(sportField.SportFieldId);

            return View(sportField);
        }

        [HttpGet]
        public async Task<List<Schedule>> Schedules(Guid id, string day)
        {
            return await _sportFieldService.GetSchedule(id, day); ;
        }

        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create()
        {
            SportFieldViewModel sportFieldViewModel = new SportFieldViewModel();
            sportFieldViewModel.Categories = await _sportFieldService.GetSportCategories();
            return View(sportFieldViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportFieldViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (ModelState.IsValid)
            {
                var correctLat = double.Parse(model.Latitude, NumberStyles.Float, CultureInfo.InvariantCulture);
                var correctLong = double.Parse(model.Longitude, NumberStyles.Float, CultureInfo.InvariantCulture);
                model.SportFieldGeoLocationLat = correctLat;
                model.SportFieldGeoLocationLong = correctLong;
                await _sportFieldService.Create(model, userId);
                return RedirectToAction(nameof(AllSportFieldsList));
            }
            return await Create();
        }
        // GET: SportFields/Edit/5
        [HttpGet]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            SportFieldCategory sportField = await _sportFieldService.Get(id);
            var user = await _userManager.GetUserAsync(User);
            if (!(await _userManager.IsInRoleAsync(user, "Admin") || sportField.ApplicationUserId != user.Id) && sportField == null)
            {
                return NotFound();
            }
            else
            {
                List<Days> days = new List<Days>();
                var workingdays = (sportField.WorkDaysE).Split(",");


                foreach (var x in workingdays)
                {
                    if (!(x == null || x.Equals("")))
                    {
                        Days day = (Days)(int.Parse(x));

                        //var enumDisplayStatus = Enum.GetName(typeof(Days), day);
                        days.Add(day);
                    }
                }
                SportFieldViewModel sportFieldViewModel = new SportFieldViewModel
                {
                    Name = sportField.Name,
                    Address = sportField.Address,
                    ContactNr = sportField.ContactNr,
                    CategoryId = sportField.CategoryId,
                    StartTime = sportField.StartTime,
                    EndTime = sportField.EndTime,
                    Categories = await _sportFieldService.GetSportCategories(),
                    Workingdays = days
                };

                ViewData["Id"] = id;
                List<SportFieldPictures> sportFieldPictures = await _sportFieldService.GetPictures(id);

                ViewData["sportFieldPictures"] = sportFieldPictures;

                return View(sportFieldViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SportFieldViewModel sportField, Guid id)
        {
            if (ModelState.IsValid)
            {
                await _sportFieldService.Update(sportField, id);
                return RedirectToAction("Details", new { id = id });
            }
            sportField.Categories = await _sportFieldService.GetSportCategories();
            ViewData["Id"] = id;

            return View(sportField);
        }

        [HttpGet("{id}")]
        public async Task<SportFieldCategory> Get(Guid id)
        {
            SportFieldCategory sportfield = await _sportFieldService.Get(id);
            return sportfield;
        }

        //Get: SportField/Delete/Guid
        [HttpDelete]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _sportFieldService.Delete(id);
            return Json(new { success = result, message = result ? "Delete successful!" : "Delete unSuccesful!" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePicture(Guid id)
        {
            bool result = await _sportFieldService.DeletePicture(id);
            return Json(new { success = result, message = result ? "Delete successful!" : "Delete unSuccesful!" });
        }
    }
}
