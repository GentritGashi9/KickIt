using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Data
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            IdentityResult roleResult1;
            IdentityResult roleResult2;

            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            var roleCheck1 = await RoleManager.RoleExistsAsync("Client");
            var roleCheck2 = await RoleManager.RoleExistsAsync("Player");


            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!roleCheck1)
            {
                roleResult1 = await RoleManager.CreateAsync(new IdentityRole("Client"));
            }
            if (!roleCheck2)
            {
                roleResult2 = await RoleManager.CreateAsync(new IdentityRole("Player"));
            }
            //user creation
            ApplicationUser user = new ApplicationUser() //password User-1
            {
                Id = "fd24f2ad-666a-4f68-8b2e-693480f24366",
                UserName = "Gentriti",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Gentrit",
                Surname = "Gashi",
                ProfileImg = "default.png",
                Location = Models.Cities.Lipjan,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("5106562c-9ca7-4a99-8883-9871dabf6447")
            };
            ApplicationUser user1 = new ApplicationUser()
            {
                Id = "1be3a521-e461-446a-861a-c96fc68d49ec",
                UserName = "Drilindi",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Drilind",
                Surname = "Olluri",
                ProfileImg = "default.png",
                Location = Models.Cities.Lipjan,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("aa96686c-7a3a-4d6f-8ae1-be0f63cc3d53")
            };
            ApplicationUser user2 = new ApplicationUser()
            {
                Id = "fd801da1-3797-43f9-8e05-174bbcfbd459",
                UserName = "Berati",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Berat",
                Surname = "Dullaj",
                ProfileImg = "default.png",
                Location = Models.Cities.Prizren,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("fa50f93d-2d5c-4993-9cd1-4e2bdbf7838e")
            };
            ApplicationUser user3 = new ApplicationUser()
            {
                Id = "2b67092b-7eaa-4567-861f-1ab79e414221",
                UserName = "Molosi",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Molos",
                Surname = "Syla",
                ProfileImg = "default.png",
                Location = Models.Cities.Prishtinë,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("d4629334-8b6a-40cd-a05d-ff38d5815891")
            };
            ApplicationUser user4 = new ApplicationUser()
            {
                Id = "2190f801-782c-400e-bbd9-8a9800eae2a1",
                UserName = "Erina",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Erine",
                Surname = "Gashi",
                ProfileImg = "default.png",
                Location = Models.Cities.Prishtinë,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("0b6e451e-de09-42fe-9e98-9e3d81ffe133")
            };
            ApplicationUser user5 = new ApplicationUser()
            {
                Id = "53646e81-a606-48a7-b324-e7a7440c708a",
                UserName = "Valoni",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Valon",
                Surname = "Fejzullahu",
                ProfileImg = "default.png",
                Location = Models.Cities.Prishtinë,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("9d98c89c-bbb4-428b-86ad-d607b72605c5")
            };
            ApplicationUser user6 = new ApplicationUser()
            {
                Id = "66920f0d-7623-4363-827a-aacec51e5ab0",
                UserName = "rilindi",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Rilind",
                Surname = "Krasniqi",
                ProfileImg = "default.png",
                Location = Models.Cities.Gjilan,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("5106562c-9ca7-4a99-8883-9871dabf6447")

            };
            ApplicationUser user7 = new ApplicationUser()
            {
                Id = "818cea40-bcba-49b3-949b-d23d31fd777c",
                UserName = "rinesa",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Rinesa",
                Surname = "Hoxha",
                ProfileImg = "default.png",
                Location = Models.Cities.Drenas,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("5106562c-9ca7-4a99-8883-9871dabf6447")
            };
            ApplicationUser user8 = new ApplicationUser()
            {
                Id = "97335f45-0c65-4461-a883-8f3ee97d5bac",
                UserName = "qendrimi",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Qendrim",
                Surname = "Rama",
                ProfileImg = "default.png",
                Location = Models.Cities.Ferizaj,
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                TeamId = Guid.Parse("aa96686c-7a3a-4d6f-8ae1-be0f63cc3d53")
            };
            ApplicationUser user9 = new ApplicationUser()
            {
                Id = "8ce6d8ad-27a0-487b-ae5b-9d63efd9696b",
                UserName = "bleona",
                Email = "user@user.com",
                EmailConfirmed = true,
                Role = "Player",
                Name = "Bleone",
                Surname = "Zeneli",
                ProfileImg = "default.png",
                RegistrationDate = DateTime.Now,
                IsBanned = false,
                Location = Models.Cities.Klinë
            };

            if (await UserManager.FindByNameAsync(user.UserName) == null
                && await UserManager.FindByNameAsync(user1.UserName) == null
                && await UserManager.FindByNameAsync(user2.UserName) == null
                && await UserManager.FindByNameAsync(user3.UserName) == null
                && await UserManager.FindByNameAsync(user4.UserName) == null
                && await UserManager.FindByNameAsync(user5.UserName) == null
                && await UserManager.FindByNameAsync(user6.UserName) == null
                && await UserManager.FindByNameAsync(user7.UserName) == null
                && await UserManager.FindByNameAsync(user8.UserName) == null
                && await UserManager.FindByNameAsync(user9.UserName) == null)
            {
                await UserManager.CreateAsync(user, "User-1");
                await UserManager.CreateAsync(user1, "User-1");
                await UserManager.CreateAsync(user2, "User-1");
                await UserManager.CreateAsync(user3, "User-1");
                await UserManager.CreateAsync(user4, "User-1");
                await UserManager.CreateAsync(user5, "User-1");
                await UserManager.CreateAsync(user6, "User-1");
                await UserManager.CreateAsync(user7, "User-1");
                await UserManager.CreateAsync(user8, "User-1");
                await UserManager.CreateAsync(user9, "User-1");
            }

            ApplicationUser SAdmin = new ApplicationUser();
            SAdmin.UserName = "Admin";
            SAdmin.Email = "Admin@Dev.com";
            SAdmin.EmailConfirmed = true;
            SAdmin.Name = "Administrator";
            SAdmin.Surname = "X";
            SAdmin.Location = Models.Cities.Prishtinë;
            SAdmin.ProfileImg = "Admin.gif";
            SAdmin.Role = "Admin";
            SAdmin.RegistrationDate = DateTime.Now;
            SAdmin.IsBanned = false;

            var i = await UserManager.FindByNameAsync(SAdmin.UserName);
            if (i == null)
            {
                var adminUser = await UserManager.CreateAsync(SAdmin, "Admin-1");
                if (adminUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(SAdmin, "Admin");
                }
            }
            else if (!i.Role.Equals("Admin"))
            {
                await UserManager.RemoveFromRoleAsync(SAdmin, i.Role);
                await UserManager.AddToRoleAsync(SAdmin, "Admin");
            }
        }
    }
}
