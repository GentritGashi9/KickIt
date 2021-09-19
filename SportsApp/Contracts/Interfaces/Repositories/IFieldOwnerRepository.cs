using System;
using System.Collections.Generic;
using SportsApp.Models;
using SportsApp.Data;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface IFieldOwnerRepository
    {
        Task<FieldOwner> GetFieldOwner(Guid? Id);
        Task<IEnumerable<FieldOwner>> GetFieldOwners();
        Task<FieldOwner> Add(FieldOwner fieldOwner);
        Task<FieldOwner> Update(FieldOwner fieldOwnerChanges);
        Task<FieldOwner> Delete(Guid? Id);
        string GetFieldOwnerName(string id);

    }
}
