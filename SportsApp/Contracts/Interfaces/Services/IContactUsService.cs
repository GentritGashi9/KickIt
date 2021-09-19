using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface IContactUsService
    {
        public Task<List<ContactUs>> Get();
        public Task<ContactUs> Get(Guid Id);
        public Task<ContactUs> Add(ContactUs contactUs);
        public Task<bool> Delete(Guid Id);
    }
}
