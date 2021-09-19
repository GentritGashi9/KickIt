using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment environment;

        public AdminController(IWebHostEnvironment _environment,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAdminService adminService)
        {
            _adminService = adminService;
            _userManager = userManager;
            environment = _environment;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ModelAdd Input { get; set; }

        [BindProperty]
        public ModelEdit InputE { get; set; }

        public string ReturnUrl { get; set; }

        public string ImgName { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class ModelEdit
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]
            public string User { get; set; }
            [Required]
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]
            public string Name { get; set; }
            [Required]
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]
            public string Surname { get; set; }
            [Required]
            public Cities Location { get; set; }
            [Required]
            public DateTime DateOfBirth { get; set; }
            public IFormFile File { get; set; }
        }

        public class ModelAdd
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]            
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]
            public string User { get; set; }
            [Required]
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]

            public string Name { get; set; }
            [Required]
            [MinLength(4)]
            [Column(TypeName = "Varchar(30)")]
            public string Surname { get; set; }

            [Required]
            public Cities Location { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [Required]
            public DateTime DateOfBirth { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Json(new { data = await _userManager.Users.ToListAsync() });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPendingFields()
        {
            return Json(new { data = await _adminService.GetPendingFields() });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            return Json(new { data = await _userManager.Users.ToListAsync() });
        }
        [HttpGet]
        public async Task<ActionResult> BanSection(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewData["UserD"]=user;
            return View();
        }
        [HttpGet]
        public ActionResult ManageUsers()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ManageFields()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ManageOwners()
        {
            return View();
        }
        #region UserFunctions
        [HttpGet]
        public async Task<JsonResult> BanUser(string id)
        {
            Guid Id = Guid.Parse(id); 
            ApplicationUser user = await _adminService.BanApplicationUser(Id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error while removing Ban from user: " + user.UserName + "!" });
            }
            return Json(new { success = true, message = "User: " + user.UserName + " Bannned successfully shortly you will be redirected!" });

        }
        [HttpGet]
        public async Task<JsonResult> UnBanUser(string id)
        {
            Guid Id = Guid.Parse(id);
            ApplicationUser user = await _adminService.UnBanApplicationUser(Id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error while removing Ban from user: "+user.UserName+" !" });
            }
            return Json(new { success = true, message = "Ban removed successfully for user: "+user.UserName+" shortly you will be redirected!" });

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "This User doesn't exist anymore!" });
            }
            bool dropNotificationResult = await _adminService.DropAllUserNotification(id);
            if (dropNotificationResult) { 
                var status = await _userManager.DeleteAsync(user);
                if (status.Succeeded)
                {
                    return Json(new { success = true, message = "Delete successful!" });
                }
                else
                {
                    string s = status.Errors.ToString();
                    return Json(new { success = false, message = " Delete was unsuccessfull!" });
                }
            }
            else
            {
                return Json(new { success = false, message = " Unable to delete this user due to his notifications!" });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteR(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            user.Role = "Player";
            await _userManager.UpdateAsync(user);
            await _userManager.RemoveFromRoleAsync(user, role);
            await _userManager.AddToRoleAsync(user, "Player");
            return Json(new { success = true, message = "Default role: 'Player' set successfully!" });
        }
        [HttpPost]
        public async Task<IActionResult> EditR(string id, string ecom, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || ecom == null || ecom.Trim() == "")
            {
                return Json(new { success = false, message = "Error while Updating" });
            }
            if (!ecom.Equals("Admin") && !ecom.Equals("Client"))
            {
                return Json(new { success = false, message = "Error you can't add other roles besides 'Admin' or 'Client' " });
            }
            user.Role = ecom;

            await _userManager.UpdateAsync(user);
            await _userManager.RemoveFromRoleAsync(user, role);
            await _userManager.AddToRoleAsync(user, ecom);
            return Json(new { success = true, message = "Update successful!" });
        }
        [HttpPost]
        public async Task<IActionResult> AddR(string id, string ecom)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || ecom == null || ecom.Trim() == "")
            {
                return Json(new { success = false, message = "Error while Adding the Role!" });
            }
            if (!ecom.Equals("Admin") && !ecom.Equals("Client"))
            {
                return Json(new { success = false, message = "Error you can't add other roles besides 'Admin' or 'Client' !" });
            }
            user.Role = ecom;
            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user, ecom);
            return Json(new { success = true, message = "Update successful!" });
        }
        public async Task<IActionResult> AddNewUser()
        {
            if (Input.File == null)
            {
                ImgName = "default.png";
            }
            else
            {
                ImgName = Input.File.FileName;
                var fileExist = Path.Combine(environment.WebRootPath, "ProfileImg", ImgName);
                if (!System.IO.File.Exists(fileExist))
                {
                    if (ImgName.EndsWith(".png") || ImgName.EndsWith(".jpg") || ImgName.EndsWith(".gif"))
                    {
                        var uploads = Path.Combine(environment.WebRootPath, "ProfileImg");
                        var filePath = Path.Combine(uploads, ImgName);
                        Input.File.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
            }
            var user = new ApplicationUser
            {
                UserName = Input.User,
                Name = Input.Name,
                Surname = Input.Surname,
                ProfileImg = ImgName,
                Email = Input.Email,
                Location = Input.Location,
                DateOfBirth = Input.DateOfBirth,
                Role = "Player"
            };
            IdentityResult result = await _userManager.CreateAsync(user,Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Player");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var resultCE = await _userManager.ConfirmEmailAsync(user, code);
                return RedirectToAction("ManageUsers");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                ErrorMessage = "Cannot create another user with already existing username:" + user.UserName + "please chose another username!";
            }

            return RedirectToAction("ManageUsers");
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewData["user"] = user;
            return View();
        }
        public async Task<IActionResult> EditUserD(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (InputE.File == null)
            {
                ImgName = user.ProfileImg;
            }
            else
            {
                ImgName = InputE.File.FileName;
                var fileExist = Path.Combine(environment.WebRootPath, "ProfileImg", ImgName);
                if (!System.IO.File.Exists(fileExist))
                {
                    if (ImgName.EndsWith(".png") || ImgName.EndsWith(".jpg") || ImgName.EndsWith(".gif"))
                    {
                        var uploads = Path.Combine(environment.WebRootPath, "ProfileImg");
                        var filePath = Path.Combine(uploads, ImgName);
                        InputE.File.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
            }
            if (InputE.Name != user.Name || InputE.Surname != user.Surname || InputE.DateOfBirth != user.DateOfBirth ||
                    InputE.Email != user.Email || InputE.User != user.UserName  || InputE.Location != user.Location)
            {
                user.UserName = InputE.User;
                user.Name = InputE.Name;
                user.Surname = InputE.Surname;
                user.Email = InputE.Email;
                user.Location = InputE.Location;
                user.ProfileImg = ImgName;
                user.DateOfBirth = InputE.DateOfBirth;
                var Userchanger = await _userManager.UpdateAsync(user);
                if (Userchanger.Succeeded && !user.EmailConfirmed)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, code);
                    return RedirectToAction("ManageUsers");
                }
            }

            return RedirectToAction("ManageUsers");
        }

        #endregion
        #region FieldFunctions
        [HttpPost]
        public async Task<JsonResult> AcceptField(string id) {
            Guid Id = Guid.Parse(id);
            var field = await _adminService.GetSpecificField(Id);
            if (field == null)
            {
                return Json(new { success = false, message = "This field: "+field.Name+" doesn't exist!" });
            }
            bool result = await _adminService.AcceptPendingSportField(field);
            if (result)
            {
                return Json(new { success = true, message = "Field accepted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "The Field was not accepted something failed!" });
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DenyField(string id)
        {
            Guid Id = Guid.Parse(id);
            var field = await _adminService.GetSpecificField(Id);
            if (field == null)
            {
                return Json(new { success = false, message = "This field: " + field.Name + " doesn't exist!" });
            }
            bool result = await _adminService.RemovePendingSportField(Id);
            if (result)
            {
                return Json(new { success = true, message = "Field accepted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "The Field was not accepted something failed!" });
            }
        }
        [HttpGet]
        public async Task<ActionResult> FieldDetails(string id)
        {
            Guid Id = Guid.Parse(id);
            SportField field = await _adminService.GetSpecificField(Id);
            List<string> paths = await _adminService.GetPathsByField(Id);
            ViewData["Field"] = field;
            ViewData["Paths"] = paths;
            return View();
        }

        #endregion
        #region OwnersFunctions
        #endregion

    }
}
