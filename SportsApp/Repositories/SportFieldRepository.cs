using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class SportFieldRepository : ISportFieldRepository
    {
        private readonly ApplicationDbContext _context;

        public SportFieldRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SportField> Create(SportField sportField)
        {
            if (sportField != null)
            {
                await _context.SportFields.AddAsync(sportField);
                await _context.SaveChangesAsync();
            }
            return sportField;
        }
        public async Task<SportField> GetSpecificSportField(Guid id)
        {
            return await _context.SportFields.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> Delete(Guid id)
        {

            SportField sportField = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == id);
            if (sportField == null)
            {
                return false;
            }
            else
            {
                _context.SportFields.Remove(sportField);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<SportFieldCategory>> Get()
        {
            List<SportFieldCategory> sportField = await (from c in _context.Categories
                                                         join s in _context.SportFields
                                                         on c.Id equals s.CategoryId
                                                         into sportfieldsGroup
                                                         from t in sportfieldsGroup
                                                         join user in _context.Users
                                                         on t.ApplicationUserId equals user.Id into SportFieldUser
                                                         from p in SportFieldUser
                                                         where t.IsApproved == true
                                                         select new SportFieldCategory
                                                         {
                                                             SportFieldId = t.Id,
                                                             Name = t.Name,
                                                             CategoryName = c.Name,
                                                             Address = t.Address,
                                                             ContactNr = t.ContactNr,
                                                             IsApproved = t.IsApproved,
                                                             WorkDaysE = t.WorkDaysE,
                                                             CategoryId = t.CategoryId,
                                                             FieldOwner = p.UserName,
                                                             MainPicture = t.MainPicture,
                                                             ApplicationUserId = t.ApplicationUserId,
                                                             SportFieldGeoLocationLat = t.SportFieldGeoLocationLat,
                                                             SportFieldGeoLocationLong = t.SportFieldGeoLocationLong
                                                         }).ToListAsync();

            return sportField;
        }
        public async Task<List<SportFieldCategory>> GetTop4()
        {
            List<SportFieldCategory> sportFields = await (from c in _context.Categories
                                                          join s in _context.SportFields
                                                          on c.Id equals s.CategoryId
                                                          into sportfieldsGroup
                                                          from t in sportfieldsGroup
                                                          join user in _context.Users
                                                          on t.ApplicationUserId equals user.Id into SportFieldUser
                                                          from p in SportFieldUser
                                                          where t.IsApproved == true
                                                          orderby t.ViewsCount descending
                                                          select new SportFieldCategory
                                                          {
                                                              SportFieldId = t.Id,
                                                              Name = t.Name,
                                                              CategoryName = c.Name,
                                                              Address = t.Address,
                                                              ContactNr = t.ContactNr,
                                                              IsApproved = t.IsApproved,
                                                              CategoryId = t.CategoryId,
                                                              FieldOwner = p.UserName,
                                                              MainPicture = t.MainPicture,
                                                              ApplicationUserId = t.ApplicationUserId
                                                          }).Take(4).ToListAsync();
            return sportFields;
        }
        public async Task<List<SportFieldCategory>> GetOwnerSportFields(string id)
        {
            List<SportFieldCategory> sportFields = await (from c in _context.Categories
                                                          join s in _context.SportFields
                                                          on c.Id equals s.CategoryId
                                                          into sportfieldsGroup
                                                          from t in sportfieldsGroup
                                                          where t.ApplicationUserId == id && t.IsApproved == true
                                                          select new SportFieldCategory
                                                          {
                                                              SportFieldId = t.Id,
                                                              Name = t.Name,
                                                              CategoryName = c.Name,
                                                              Address = t.Address,
                                                              ContactNr = t.ContactNr,
                                                              ApplicationUserId = t.ApplicationUserId
                                                          }).ToListAsync();
            return sportFields;
        }
        public async Task<List<SportFieldCategory>> GetSpecificWithout(string id)
        {
            List<SportFieldCategory> sportFields = await (from c in _context.Categories
                                                          join s in _context.SportFields
                                                          on c.Id equals s.CategoryId
                                                          into sportfieldsGroup
                                                          from t in sportfieldsGroup
                                                          where t.ApplicationUserId != id && t.IsApproved == true
                                                          select new SportFieldCategory
                                                          {
                                                              SportFieldId = t.Id,
                                                              Name = t.Name,
                                                              CategoryName = c.Name,
                                                              Address = t.Address,
                                                              ContactNr = t.ContactNr,
                                                              ApplicationUserId = t.ApplicationUserId
                                                          }).ToListAsync();

            return sportFields;
        }

        public async Task<List<SportFieldCategory>> GetSpecificFieldsPending(string id)
        {
            List<SportFieldCategory> sportFields = await (from c in _context.Categories
                                                          join s in _context.SportFields
                                                          on c.Id equals s.CategoryId
                                                          into sportfieldsGroup
                                                          from t in sportfieldsGroup
                                                          where t.ApplicationUserId == id && t.IsApproved == false
                                                          select new SportFieldCategory
                                                          {
                                                              SportFieldId = t.Id,
                                                              Name = t.Name,
                                                              CategoryName = c.Name,
                                                              Address = t.Address,
                                                              ContactNr = t.ContactNr,
                                                              ApplicationUserId = t.ApplicationUserId
                                                          }).ToListAsync();
            return sportFields;
        }
        public async Task<SportFieldCategory> Get(Guid id)
        {
            SportFieldCategory sportField = await (
                                                    from c in _context.Categories
                                                    join s in _context.SportFields
                                                         on c.Id equals s.CategoryId into SportFieldCategory
                                                    from sc in SportFieldCategory.DefaultIfEmpty()
                                                    join user in _context.Users
                                                         on sc.ApplicationUserId equals user.Id into SportFieldUser
                                                    from p in SportFieldUser.DefaultIfEmpty()
                                                    orderby sc.Name descending
                                                    select new SportFieldCategory
                                                    {
                                                        SportFieldId = sc.Id,
                                                        Name = sc.Name,
                                                        CategoryName = c.Name,
                                                        Address = sc.Address,
                                                        ContactNr = sc.ContactNr,
                                                        CategoryId = sc.CategoryId,
                                                        IsApproved = sc.IsApproved,
                                                        ViewsCount = sc.ViewsCount,
                                                        WorkDaysE = sc.WorkDaysE,
                                                        ApplicationUserId = sc.ApplicationUserId,
                                                        FieldOwner = p.UserName,
                                                        StartTime = sc.StartTime,
                                                        EndTime = sc.EndTime

                                                    }).FirstOrDefaultAsync(x => x.SportFieldId == id);
            return sportField;
        }

        public async Task<SportField> Update(SportField sportfieldChanges)
        {
            SportField sportfield = await _context.SportFields.FirstOrDefaultAsync(x => x.Id == sportfieldChanges.Id);
            if (sportfield != null)
            {
                sportfield.Name = sportfieldChanges.Name;
                sportfield.Address = sportfieldChanges.Address;
                sportfield.ContactNr = sportfieldChanges.ContactNr;
                sportfield.CategoryId = sportfieldChanges.CategoryId;
                sportfield.WorkDaysE = sportfieldChanges.WorkDaysE;
                sportfield.StartTime = sportfieldChanges.StartTime;
                sportfield.EndTime = sportfieldChanges.EndTime;
                if(sportfield.WorkDaysE != sportfieldChanges.WorkDaysE && sportfieldChanges.WorkDaysE != null)
                {
                    sportfield.WorkDaysE = sportfieldChanges.WorkDaysE;
                }
            }
            await _context.SaveChangesAsync();
            return sportfield;
        }

        public void AddViewCount(Guid sportFieldId)
        {
            SportField sportField = _context.SportFields.FirstOrDefault(x => x.Id == sportFieldId);
            sportField.ViewsCount++;
            _context.SaveChangesAsync();
        }

        public async Task<SportField> changeMainPicture(Guid sportFieldId, string mainPicturePath)
        {
            SportField sportField =  await  _context.SportFields.FirstOrDefaultAsync(x => x.Id == sportFieldId);
            sportField.MainPicture = mainPicturePath;
            _context.SportFields.Update(sportField);
            await _context.SaveChangesAsync();
            return sportField;
        }

        public async Task<List<SportFieldCategory>> GetSportfieldFromCategory(Guid categoryId)
        {
            List<SportFieldCategory> sportField = await(from c in _context.Categories
                                                        join s in _context.SportFields
                                                        on c.Id equals s.CategoryId
                                                        into sportfieldsGroup
                                                        from t in sportfieldsGroup
                                                        join user in _context.Users
                                                        on t.ApplicationUserId equals user.Id into SportFieldUser
                                                        from p in SportFieldUser
                                                        where t.IsApproved == true && t.CategoryId == categoryId
                                                        select new SportFieldCategory
                                                        {
                                                            SportFieldId = t.Id,
                                                            Name = t.Name,
                                                            CategoryName = c.Name,
                                                            Address = t.Address,
                                                            ContactNr = t.ContactNr,
                                                            IsApproved = t.IsApproved,
                                                            CategoryId = t.CategoryId,
                                                            FieldOwner = p.UserName,
                                                            MainPicture = t.MainPicture,
                                                            ApplicationUserId = t.ApplicationUserId,
                                                            SportFieldGeoLocationLat = t.SportFieldGeoLocationLat,
                                                            SportFieldGeoLocationLong = t.SportFieldGeoLocationLong
                                                        }).ToListAsync();

            return sportField;
        }
    }
}
