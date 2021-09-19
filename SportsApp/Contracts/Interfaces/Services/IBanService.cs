using SportsApp.Data;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IBanService
    {
        public Task<ApplicationUser> BanPlayer(SportField sportField, ApplicationUser user);
        public Task<ApplicationUser> PermaBanPlayer(ApplicationUser user);
        public Task<List<Ban>> GetAllBanned();
        public Task<ApplicationUser> RemovePlayerBan(SportField sportField, ApplicationUser user);
        public Task<SportField> RemovePlayerPermaBan(ApplicationUser user);
    }
}
