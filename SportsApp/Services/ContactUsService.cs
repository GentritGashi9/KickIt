using Microsoft.AspNetCore.Mvc;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class ContactUsService : IContactUsService
    {

        private readonly IContactUsRepository _contactUsRepository;

        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }
        public Task<ContactUs> Add(ContactUs contactUs)
        {
            return _contactUsRepository.Add(contactUs);
        }

        public Task<bool> Delete(Guid Id)
        {
            return _contactUsRepository.Delete(Id);
        }

        public Task<List<ContactUs>> Get()
        {
            return _contactUsRepository.Get();
        }

        public Task<ContactUs> Get(Guid Id)
        {
            return _contactUsRepository.Get(Id);
        }
    }
}
