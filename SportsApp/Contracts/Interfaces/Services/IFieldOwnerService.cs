using SportsApp.Models;
using SportsApp.Views.SportField;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IFieldOwnerService
    {
        Task<FieldOwner> GetFieldOwner(Guid? Id);
        Task<IEnumerable<FieldOwner>> GetFieldOwners();
        Task<FieldOwner> Add(FieldOwner fieldOwner);
        Task<FieldOwner> Update(FieldOwner fieldOwnerChanges);
        Task<FieldOwner> Delete(Guid? Id);
        string GetFieldOwnerName(string id);

    }
}
