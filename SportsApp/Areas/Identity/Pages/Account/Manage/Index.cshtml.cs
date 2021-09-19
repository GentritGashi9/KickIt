using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsApp.Data;

namespace SportsApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment environment;


        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            environment = hostingEnvironment;
        }

        public string Username { get; set; }
        public string ImgName { get; set; }

        public DateTime DateOfBirth { get; set; } 

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public IFormFile File { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            

            Username = userName;

            DateOfBirth = user.DateOfBirth.Date;


            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ViewData["UserProfilImg"] = user.ProfileImg;
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.File == null)
            {
                ImgName = user.ProfileImg;
            }
            else
            {
                ImgName = Input.File.FileName;
                var fileExist = Path.Combine(environment.WebRootPath, "ProfileImg", ImgName);
                if (!System.IO.File.Exists(fileExist))
                {

                    if (ImgName.EndsWith(".png") || ImgName.EndsWith(".jpg") || ImgName.EndsWith(".gif"))
                    {
                        //var uniqueFileName = GetUniqueFileName(Image.file.FileName);
                        var uploads = Path.Combine(environment.WebRootPath, "ProfileImg");
                        var filePath = Path.Combine(uploads, ImgName);
                        Input.File.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
            }
            user.ProfileImg = ImgName;
            await _userManager.UpdateAsync(user);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                if (Input.PhoneNumber == "000 000 000")
                {
                    StatusMessage = "Error invalid phone number.";
                    return RedirectToPage();
                }
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }

            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
