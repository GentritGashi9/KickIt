using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;

namespace SportsApp.Repositories
{
    public class FieldOwnerRepository : IFieldOwnerRepository
    {
        private readonly ApplicationDbContext context;

        public FieldOwnerRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<FieldOwner> Add(FieldOwner fieldOwner)
        {
            await context.AddAsync(fieldOwner);
            await context.SaveChangesAsync();
            return fieldOwner;
        }

        public async Task<FieldOwner> Delete(Guid? Id)
        {
            FieldOwner fieldOwner = await context.FieldOwners.FindAsync(Id);
            if (fieldOwner != null)
            {
                context.FieldOwners.Remove(fieldOwner);
                await context.SaveChangesAsync();
            }
            return fieldOwner;
        }

        public async Task<FieldOwner> GetFieldOwner(Guid? Id)
        {
            
            return await context.FieldOwners.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<FieldOwner>> GetFieldOwners()
        {
            return await context.FieldOwners.ToListAsync();
        }

        public async Task<FieldOwner> Update(FieldOwner fieldOwnerChanges)
        {
            var fieldOwner = context.FieldOwners.Attach(fieldOwnerChanges);
            fieldOwner.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
            return fieldOwnerChanges;
        }

        public string GetFieldOwnerName(string id)
        {
            ApplicationUser applicationUser = context.Users.FirstOrDefault(x => x.Id == id);
            return applicationUser.Name;
        }
    }
}