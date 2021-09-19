using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsApp.Models;
using System;
using System.Reflection;

namespace SportsApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<GameRoomChat> GameRoomChats { get; set; }
        public DbSet<FieldOwner> FieldOwners { get; set; }
        public DbSet<SportField> SportFields { get; set; }
        public DbSet<SportFieldPictures> SportFieldPictures { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamRequests> TeamsRequests { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Matches> Matches { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Team>()
                .HasMany(a => a.Players)
                .WithOne(b => b.Team)
                .HasForeignKey(b => b.TeamId).OnDelete(DeleteBehavior.SetNull);


            Guid footballId = Guid.NewGuid();
            Guid basketballId = Guid.NewGuid();
            Guid volleyballId = Guid.NewGuid();
            Guid tennisId = Guid.NewGuid();


            //SEEDS
            builder.Entity<Category>().HasData(
                new Category { Id = footballId, Name = "Football", MaxCapacity = 11, MinCapacity = 4 },
                new Category { Id = basketballId, Name = "Basketball", MaxCapacity = 5, MinCapacity = 2 },
                new Category { Id = volleyballId, Name = "Volleyball", MaxCapacity = 6, MinCapacity = 2 },
                new Category { Id = tennisId, Name = "Tennis", MaxCapacity = 2, MinCapacity = 1 }
            );

            builder.Entity<Team>().HasData(
                new Team { Id = Guid.Parse("5106562c-9ca7-4a99-8883-9871dabf6447"), Name = "ErwinHype", City = Cities.Prishtinë, CategoryId = footballId, TeamLeaderId = Guid.Parse("fd24f2ad-666a-4f68-8b2e-693480f24366"), isPrivate = true },
                new Team { Id = Guid.Parse("aa96686c-7a3a-4d6f-8ae1-be0f63cc3d53"), Name = "Thor", City = Cities.Prishtinë, CategoryId = footballId, TeamLeaderId = Guid.Parse("1be3a521-e461-446a-861a-c96fc68d49ec") },
                new Team { Id = Guid.Parse("fa50f93d-2d5c-4993-9cd1-4e2bdbf7838e"), Name = "Hela", City = Cities.Lipjan, CategoryId = footballId, TeamLeaderId = Guid.Parse("fd801da1-3797-43f9-8e05-174bbcfbd459") },
                new Team { Id = Guid.Parse("d4629334-8b6a-40cd-a05d-ff38d5815891"), Name = "Odin", City = Cities.Prishtinë, CategoryId = basketballId, TeamLeaderId = Guid.Parse("2b67092b-7eaa-4567-861f-1ab79e414221"), isPrivate = true },
                new Team { Id = Guid.Parse("0b6e451e-de09-42fe-9e98-9e3d81ffe133"), Name = "Frigga", City = Cities.Prishtinë, CategoryId = basketballId, TeamLeaderId = Guid.Parse("2190f801-782c-400e-bbd9-8a9800eae2a1") },
                new Team { Id = Guid.Parse("9d98c89c-bbb4-428b-86ad-d607b72605c5"), Name = "Sylvie", City = Cities.Prishtinë, CategoryId = tennisId, TeamLeaderId = Guid.Parse("53646e81-a606-48a7-b324-e7a7440c708a") }
            );
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<SportsApp.Models.ContactUs> ContactUs { get; set; }
    }
}
