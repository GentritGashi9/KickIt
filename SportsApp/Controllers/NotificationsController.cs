using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public NotificationsController(INotificationService context,UserManager<ApplicationUser> userManager)
        {
            _notificationService = context;
            _userManager = userManager;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _notificationService.Get();
            return View(applicationDbContext);
        }
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var unReadNotifications = await _notificationService.GetUnReadNotification(user.Id);
            var readNotifications = await _notificationService.GetReadNotification(user.Id);

            return Json(new { notificationNR = unReadNotifications, notificationR = readNotifications });
        }
        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var notification = await _notificationService.Get(id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,IsRead,UserId")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                await _notificationService.Add(notification);
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var notification = await _notificationService.Get(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Type,IsRead,UserId")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _notificationService.Update(notification);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await NotificationExists(notification.UserId, notification.TeamId)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            bool result = await _notificationService.Delete(id);
            return Json(new { success = result, message = result ? "Delete Successful!" : "Delete Unsuccesful!" });        
        }

        public async Task<IActionResult> RefuseInvitation(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            bool result = await _notificationService.Delete(id);
            return Json(new { success = result, message = result ? "Invitation Refused!" : "Invitation Refused Unsuccesfully!" });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveUsless(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            bool result = await _notificationService.Delete(id);
            return Json(new { success = result, message = result ? "Match notification removed successfully!" : "Match notification wasn't removed!" });
        }
        private async Task<bool> NotificationExists(string userId, Guid teamId)
        {
            return await _notificationService.NotificationExists(userId, teamId);
        }
    }
}
