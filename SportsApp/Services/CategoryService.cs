using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository= categoryRepository;
        }
       
        public Task<Category> Add(Category category)
        {
            return _categoryRepository.Add(category);
        }

        public Task<bool> Delete(Guid Id)
        {
           return _categoryRepository.Delete(Id);
        }

        public Task<List<Category>> Get()
        {
            return _categoryRepository.Get();
        }

        public Task<Category> Get(Guid Id)
        {
            return _categoryRepository.Get(Id);
        }

        public Task<Category> Update(Category category)
        {
            return _categoryRepository.Update(category);
        }
        
        public Task<bool> CategoryExists(Guid id){
           return _categoryRepository.CategoryExists(id);
        }

        public async Task<string> GetName(Guid id)
        {
            return await _categoryRepository.GetName(id);
        }
    }
}
