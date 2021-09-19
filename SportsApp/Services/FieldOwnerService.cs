using Microsoft.AspNetCore.Hosting;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class FieldOwnerService : IFieldOwnerService
    {
        private readonly IFieldOwnerRepository _fieldOwnerRepository;
        public FieldOwnerService(IFieldOwnerRepository fieldOwnerRepository)
        {
            _fieldOwnerRepository = fieldOwnerRepository;
        }
        public async Task<FieldOwner> Add(FieldOwner fieldOwner)
        {
            return await _fieldOwnerRepository.Add(fieldOwner);
        }

        public async Task<FieldOwner> Delete(Guid? Id)
        {
            return await _fieldOwnerRepository.Delete(Id);
        }

        public async Task<FieldOwner> GetFieldOwner(Guid? Id)
        {
            return await _fieldOwnerRepository.GetFieldOwner(Id);
        }

        public async Task<IEnumerable<FieldOwner>> GetFieldOwners()
        {
            return await _fieldOwnerRepository.GetFieldOwners();
        }

        public async Task<FieldOwner> Update(FieldOwner fieldOwnerChanges)
        {
            return await _fieldOwnerRepository.Update(fieldOwnerChanges);
        }
        public  string GetFieldOwnerName(string id)
        {
            return _fieldOwnerRepository.GetFieldOwnerName(id);
        }
    }
}
