using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<bool> AcceptPendingSportField(SportField sportField)
        {
            return await _adminRepository.AcceptPendingSportField(sportField);
        } 
        public async Task<bool> RemovePendingSportField(Guid id)
        {
            return await _adminRepository.RemovePendingSportField(id);
        }
        public async Task<List<SportFieldCategory>> GetPendingFields()
        {
            return await _adminRepository.GetPendingFields();
        }
        public async Task<SportField> GetSpecificField(Guid id)
        {
            return await _adminRepository.GetSpecificField(id);
        }
        public async Task<List<string>> GetPathsByField(Guid id)
        {
            List<SportFieldPictures> pics = await _adminRepository.GetPathsByField(id);

            List<string> paths = new List<string>();

            foreach (SportFieldPictures picture in pics)
            {
                paths.Add(picture.Path);
            }
            return paths;
            
        }

        public async Task<ApplicationUser> BanApplicationUser(Guid id)
        {
            return await _adminRepository.BanApplicationUser(id);
        }
        public async Task<ApplicationUser> UnBanApplicationUser(Guid id)
        {
            return await _adminRepository.UnBanApplicationUser(id);
        }
        public async Task<bool> DropAllUserNotification(string id)
        {
            return await _adminRepository.DropAllUserNotification(id);
        }
    }
}
