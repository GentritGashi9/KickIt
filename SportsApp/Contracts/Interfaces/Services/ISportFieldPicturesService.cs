using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface ISportFieldPicturesService
    {
        public Task<SportFieldPictures> AddNewSportFieldPicture(SportFieldPictures sportFieldPicture);
        public Task<string> UploadedFile(SportFieldViewModel model, Guid id);
        public Task<List<SportFieldPictures>> GetPicturesByField(Guid id);
        public Task<bool> DeletePicture(Guid id);
        public Task<bool> DeletePictures(Guid id);
    }
}
