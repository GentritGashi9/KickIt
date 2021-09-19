using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;

namespace SportsApp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _context;
        private readonly IEmailSender _emailSender;

        public ContactUsController(IContactUsService context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: ContactUs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(new { data = await _context.Get() });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllContacts()
        {
            return View(await _context.Get());
        }

        // GET: ContactUs/Details/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var contactUs = await _context.Get(id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendResponse(ContactUs contactUs)
        {
            if (contactUs == null)
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }
            else
            {
                if(contactUs.Email == null || contactUs.Title == "" || contactUs.Message == "")
                {
                    return Json(new { success = false, message = "Fields can not be empty!" });
                }
                await _emailSender.SendEmailAsync(contactUs.Email, contactUs.Title,
                    $"Administrator response <b>{contactUs.Message}</b>.");
                return NoContent();
            }
        }

        // GET: ContactUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Title,Message")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                contactUs.Id = Guid.NewGuid();
                await _context.Add(contactUs);
                return View(contactUs);
            }
            return View(contactUs);
        }

        // GET: ContactUs/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await _context.Delete(id);
            return Json(new { success = result, message = result ? "Delete successful!" : "Delete unSuccesful!" });
        }
    }
}
