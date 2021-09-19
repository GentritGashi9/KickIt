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
    public class SportFieldPicturesRepository : ISportFieldPicturesRepository
    {
        private readonly ApplicationDbContext _context;

        public SportFieldPicturesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SportFieldPictures> AddNewSportFieldPicture(SportFieldPictures sportFieldPicture)
        {
            if (sportFieldPicture != null)
            {
                await _context.SportFieldPictures.AddAsync(sportFieldPicture);
                await _context.SaveChangesAsync();
            }
            return sportFieldPicture;
        }

        public async Task<bool> Delete(Guid id)
        {
            SportFieldPictures sportFieldPicutre  =  await _context.SportFieldPictures.FirstOrDefaultAsync(x => x.Id == id);
            if(sportFieldPicutre != null)
            {
                _context.SportFieldPictures.Remove(sportFieldPicutre);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<SportFieldPictures>> GetPicturesByField(Guid id)
        {
            if(id.ToString() == null)
            {
                return null;
            }
            return await _context.SportFieldPictures.Where(x => x.SportFieldId == id).ToListAsync();
        }

        public async Task<SportFieldPictures> GetSportFieldPicture(Guid id)
        {
            return await _context.SportFieldPictures.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
