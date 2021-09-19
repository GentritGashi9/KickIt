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
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        
        {
            _context = context;   
        }
        public async Task<Category> Add(Category category)
        {
           if(category != null)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
               
            }
            return category;
        }

        public async Task<bool> Delete(Guid Id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Id);

            if(category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Category>> Get()
        {
            return await _context.Categories.ToListAsync();

        }

        public async Task<Category> Get(Guid Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x=> x.Id == Id);

        }

        public async Task<Category> Update(Category category)
        {
            var categoryChanges = _context.Categories.Attach(category);
            categoryChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> CategoryExists(Guid id){
            return await _context.Categories.AnyAsync(e => e.Id == id);
        }

        public async Task<string> GetName(Guid id)
        {
            return (await _context.Categories.FirstOrDefaultAsync(c => c.Id == id)).Name;
        }
    }
}
