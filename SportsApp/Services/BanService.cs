using Microsoft.AspNetCore.Hosting;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class BanService : IBanService
    {
        private readonly IBanRepository _banRepository;
        public BanService(IBanRepository banRepo)
        {
            _banRepository = banRepo;
        }
        public Task<ApplicationUser> BanPlayer(SportField sportField, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> PermaBanPlayer(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ban>> GetAllBanned()
        {
            throw new NotImplementedException();
        }
        
        public Task<ApplicationUser> RemovePlayerBan(SportField sportField, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<SportField> RemovePlayerPermaBan(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}

