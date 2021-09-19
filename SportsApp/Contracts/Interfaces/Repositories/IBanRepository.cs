using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface IBanRepository
    {
        public Task<ApplicationUser> Ban(SportField sportField,ApplicationUser user);
        public Task<List<Ban>> GetAllBannedPlayers();
        public Task<ApplicationUser> RemoveBan(SportField sportField, ApplicationUser user);
        public Task<SportField> RemovePermaBan(ApplicationUser user);
    }
}
