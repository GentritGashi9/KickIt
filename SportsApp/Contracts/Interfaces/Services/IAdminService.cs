using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IAdminService
    {
        public Task<bool> AcceptPendingSportField(SportField sportField);
        public Task<List<SportFieldCategory>> GetPendingFields();
        public Task<SportField> GetSpecificField(Guid id);
        public Task<List<string>> GetPathsByField(Guid id);
        public Task<bool> RemovePendingSportField(Guid id);
        public Task<ApplicationUser> BanApplicationUser(Guid id);
        public Task<ApplicationUser> UnBanApplicationUser(Guid id);
        public Task<bool> DropAllUserNotification(string id);


    }
}
