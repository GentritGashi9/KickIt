using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> Get();
        Task<Category> Get(Guid Id);
        Task<bool> Delete(Guid Id);
        Task<Category> Update(Category category);
        Task<Category> Add(Category category);
        Task<bool> CategoryExists(Guid id);
        Task<string> GetName(Guid id);
    }
}
