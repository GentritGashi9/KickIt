using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class SportFieldPicturesService : ISportFieldPicturesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISportFieldPicturesRepository _sportFieldPicturesRepository;

        public SportFieldPicturesService(IWebHostEnvironment webHostEnvironment, ISportFieldPicturesRepository sportFieldPicturesRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _sportFieldPicturesRepository = sportFieldPicturesRepository;
        }

        public async Task<SportFieldPictures> AddNewSportFieldPicture(SportFieldPictures sportFieldPicture)
        {
            return await _sportFieldPicturesRepository.AddNewSportFieldPicture(sportFieldPicture);
        }

        public async Task<bool> DeletePicture(Guid id)
        {
            SportFieldPictures sportFieldPicture = await _sportFieldPicturesRepository.GetSportFieldPicture(id);
            string path = sportFieldPicture.Path;

            bool deleted = false;
            string picturesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/sportFieldPictures");

            if (File.Exists(Path.Combine(picturesFolder, path)))
            {
                // If file found, delete it    
                File.Delete(Path.Combine(picturesFolder, path));

                deleted = !File.Exists(Path.Combine(picturesFolder, path));
            }

            return await _sportFieldPicturesRepository.Delete(id) && deleted;
        }

        public async Task<bool> DeletePictures(Guid id)
        {
            List<SportFieldPictures> sportFieldPictures = await _sportFieldPicturesRepository.GetPicturesByField(id);


            foreach (SportFieldPictures sportFieldPicture in sportFieldPictures)
            {
                await DeletePicture(sportFieldPicture.Id);

            }
            return true;
        }

        public async Task<List<SportFieldPictures>> GetPicturesByField(Guid id)
        {
            List<SportFieldPictures> sportFieldPictures = await _sportFieldPicturesRepository.GetPicturesByField(id);
            return sportFieldPictures;
        }

        public async Task<string> UploadedFile(SportFieldViewModel model, Guid id)
        {
            string uniqueFileName = null;

            if (model.Pictures != null && model.Pictures.Count > 0)
            {
                foreach (IFormFile picture in model.Pictures)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/sportFieldPictures");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        picture.CopyTo(fileStream);
                    }
                    SportFieldPictures sportFieldPictures = new SportFieldPictures()
                    {
                        Id = Guid.NewGuid(),
                        Path = uniqueFileName,
                        SportFieldId = id
                    };
                    await AddNewSportFieldPicture(sportFieldPictures);
                }
            }
            return uniqueFileName;
        }
    }
}
