using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface ISportFieldPicturesRepository
    {
        public Task<SportFieldPictures> AddNewSportFieldPicture(SportFieldPictures sportFieldPicture);
        public Task<List<SportFieldPictures>> GetPicturesByField(Guid id);
        public Task<SportFieldPictures> GetSportFieldPicture(Guid id); 
        public Task<bool> Delete(Guid id );
    }
}
