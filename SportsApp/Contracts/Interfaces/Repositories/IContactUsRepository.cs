using Microsoft.AspNetCore.Mvc;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface IContactUsRepository
    {
        public Task<List<ContactUs>> Get();
        Task<ContactUs> Get(Guid Id);
        public Task<ContactUs> Add(ContactUs contactUs);
        public Task<bool> Delete(Guid Id);
    }
}
