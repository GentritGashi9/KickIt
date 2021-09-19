using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface ISportFieldRepository
    {
        Task<SportField> Create(SportField sportField);
        Task<List<SportFieldCategory>> Get();
        Task<List<SportFieldCategory>> GetSportfieldFromCategory(Guid categoryId);
        Task<List<SportFieldCategory>> GetTop4();
        Task<List<SportFieldCategory>> GetOwnerSportFields(string id);
        Task<List<SportFieldCategory>> GetSpecificFieldsPending(string id);
        Task<List<SportFieldCategory>> GetSpecificWithout(string id);
        Task<SportFieldCategory> Get(Guid id);
        Task<SportField> GetSpecificSportField(Guid id);
        Task<bool> Delete(Guid id);
        Task<SportField> Update(SportField sportfieldChanges);
        void AddViewCount(Guid sportFieldId);
        Task<SportField> changeMainPicture(Guid sportFieldId, string mainPicturePath);

    }
}
