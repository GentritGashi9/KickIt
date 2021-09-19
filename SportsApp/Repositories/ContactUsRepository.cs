using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactUsRepository(ApplicationDbContext context)

        {
            _context = context;
        }

        public async Task<ContactUs> Add(ContactUs contactUs)
        {
            if (contactUs != null)
            {
                await _context.ContactUs.AddAsync(contactUs);
                await _context.SaveChangesAsync();

            }
            return contactUs;
        }

        public async Task<bool> Delete(Guid Id)
        {
            ContactUs contactUs = await _context.ContactUs.FirstOrDefaultAsync(x => x.Id == Id);

            if (contactUs != null)
            {
                _context.ContactUs.Remove(contactUs);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ContactUs>> Get()
        {
            return await _context.ContactUs.ToListAsync();
        }

        public async Task<ContactUs> Get(Guid Id)
        {
            return await _context.ContactUs.FirstOrDefaultAsync(x => x.Id == Id);
        }

    }
}
