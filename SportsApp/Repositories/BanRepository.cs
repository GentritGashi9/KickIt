using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class BanRepository : IBanRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BanRepository(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<ApplicationUser> Ban(SportField sportField, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<SportField> Create(SportField sportField)
        {
            if(sportField != null) { 
                await _context.SportFields.AddAsync(sportField);
                await _context.SaveChangesAsync();
            }
            return sportField;
        }
       
        public async Task<bool> Delete(Guid id)
        {

            SportField sportField = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == id);
            if(sportField == null)
            {
                return false;
            }
            else { 
                  _context.SportFields.Remove(sportField);
                    await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<SportField>> Get()
        {
            return await _context.SportFields.ToListAsync();
        }

        public async Task<SportField> Get(Guid id)
        {
            return await _context.SportFields.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Ban>> GetAllBannedPlayers()
        {
            throw new NotImplementedException();
        }
        public Task<ApplicationUser> RemoveBan(SportField sportField, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<SportField> RemovePermaBan(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<SportField> Update(SportField sportfieldChanges)
        {
            SportField sf = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == sportfieldChanges.Id);
            if(sf != null)
            {
                sf.Name = sportfieldChanges.Name;
                sf.Address = sportfieldChanges.Address;
                sf.ContactNr = sportfieldChanges.ContactNr;
            }
            await _context.SaveChangesAsync();
            return sf;
        }
    }
}
