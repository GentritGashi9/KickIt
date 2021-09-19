using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;

namespace SportsApp.Controllers
{
    [Authorize]
    public class FieldOwnerController : Controller
    {
        private readonly IFieldOwnerService _fieldOwnerService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public FieldOwnerController(IFieldOwnerService context, UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> signInManager)
        {
            _fieldOwnerService = context;
            userManager = _userManager;
            this.signInManager = signInManager;
        }
        [Authorize(Roles = "Admin")]
        public  IActionResult GetFieldOwners()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // GET: FieldOwner
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(new { data = await _fieldOwnerService.GetFieldOwners() });
        }

        // GET: FieldOwner/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            FieldOwner fieldOwner = await _fieldOwnerService.GetFieldOwner(Id);
            ViewData["AplicationUserName"] = _fieldOwnerService.GetFieldOwnerName(fieldOwner.ApplicationUserId);
            if (fieldOwner == null)
            {
                return NotFound();
            }
            return View(fieldOwner);
        }


        // GET: FieldOwner/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = userManager.GetUserId(User);
            ViewData["ApplicationUserId"] = userId;
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Client"))
            {
                return RedirectToAction(nameof(NotFound));
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FieldOwner fieldOwner)
        {
            var userId = userManager.GetUserId(User);
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (await userManager.IsInRoleAsync(user, "Client"))
            {
                return RedirectToAction(nameof(NotFound));
            }
            else
            {
                user.Role = "Client";
                await userManager.RemoveFromRoleAsync(user, "Player");
                await userManager.AddToRoleAsync(user, "Client");
                await userManager.UpdateAsync(user);
                FieldOwner result = await _fieldOwnerService.Add(fieldOwner);
                await signInManager.RefreshSignInAsync(user);

                return RedirectToAction("Index", "Home");

            }
        }


        [Authorize(Roles = "Admin")]
        // GET: FieldOwner/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var userId = userManager.GetUserId(User);
            ViewData["ApplicationUserId"] = userId;

            var fieldOwner = await _fieldOwnerService.GetFieldOwner(id);
            return View(fieldOwner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FieldOwner fieldOwner)
        {
            await _fieldOwnerService.Update(fieldOwner);
            return RedirectToAction(nameof(GetFieldOwners));
        }

        // GET: FieldOwner/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await _fieldOwnerService.Delete(id);
            return Json(new { success = true, message = true ? "Delete successful!" : "Delete unSuccesful!" });
        }

        // POST: FieldOwner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fieldOwner = await _fieldOwnerService.Delete(id);
            return RedirectToAction(nameof(GetFieldOwners));
        }
    }
}
