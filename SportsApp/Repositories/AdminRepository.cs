using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace SportsApp.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminRepository(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Fields
        public async Task<bool> AcceptPendingSportField(SportField sportField)
        {
            sportField.IsApproved = true; 
                      
            if (sportField != null)
            {
                _context.SportFields.Update(sportField);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RemovePendingSportField(Guid id)
        {
            SportField sportField = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == id);
            List<SportFieldPictures> pictures = await _context.SportFieldPictures.Where(x => x.SportFieldId == id).ToListAsync();
            List<Schedule> schedules = await _context.Schedules.Where(x => x.SportFieldId == id).ToListAsync();
            var status = "";
            if (sportField == null)
            {
                return false;
            }
            else
            {
                foreach(Schedule s in schedules)
                {
                   var state = _context.Schedules.Remove(s);
                   status = state.State.ToString();
                }
                foreach(SportFieldPictures p in pictures)
                {
                    DeletePicture(p);
                }
                if (status.Equals("Deleted"))
                {
                    _context.SportFields.Remove(sportField);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
        public async Task<List<SportFieldCategory>> GetPendingFields()
        {
            List<SportFieldCategory> sportFields = await (from c in _context.Categories
                                                          join s in _context.SportFields
                                                          on c.Id equals s.CategoryId
                                                          into sportfieldsGroup
                                                          from t in sportfieldsGroup
                                                          where  t.IsApproved == false
                                                          select new SportFieldCategory
                                                          {
                                                              SportFieldId = t.Id,
                                                              Name = t.Name,
                                                              CategoryName = c.Name,
                                                              Address = t.Address,
                                                              ContactNr = t.ContactNr,
                                                              ApplicationUserId = t.ApplicationUserId
                                                          }).ToListAsync();

            return sportFields;
        }
        public async Task<SportField> GetSpecificField(Guid id)
        {
            return await _context.SportFields.FindAsync(id);
        }
        public async Task<List<SportFieldPictures>> GetPathsByField(Guid id)
        {
            if (id.ToString() == null)
            {
                return null;
            }
            return await _context.SportFieldPictures.Where(x => x.SportFieldId == id).ToListAsync();
        }
        public bool DeletePicture(SportFieldPictures picture)
        {
            string path = picture.Path;

            bool deleted = false;
            string picturesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/sportFieldPictures");

            if (File.Exists(Path.Combine(picturesFolder, path)))
            {
                File.Delete(Path.Combine(picturesFolder, path));

                deleted = !File.Exists(Path.Combine(picturesFolder, path));
            }
            return deleted;
        }
        #endregion
        #region BanUser 
        public async Task<ApplicationUser> BanApplicationUser(Guid id){
            ApplicationUser applicationUser = await _context.Users.FirstAsync(x => x.Id.Equals(id.ToString()));
            applicationUser.IsBanned = true;
            await _context.SaveChangesAsync();
            return applicationUser;
        }
        public async Task<ApplicationUser> UnBanApplicationUser(Guid id)
        {
            ApplicationUser applicationUser = await _context.Users.FirstAsync(x => x.Id.Equals(id.ToString()));
            applicationUser.IsBanned = false;
            await _context.SaveChangesAsync();
            return applicationUser;
        }
        public async Task<bool> DropAllUserNotification(string id)
        {
            List<Notification> userNotification = await _context.Notifications.Where(x => x.UserId == id).ToListAsync();
            if(userNotification == null)
            {
                return true;
            }
            if (userNotification.Count == 0)
            {
                return true;
            }
            var status="";
            foreach(Notification n in userNotification)
            {
                var statusr = _context.Notifications.Remove(n);
                status = statusr.State.ToString();
            }
            if (status.Equals("Deleted"))
            {
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}