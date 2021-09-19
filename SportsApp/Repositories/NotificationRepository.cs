using Microsoft.EntityFrameworkCore;
using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> Add(Notification notification)
        {
            if (notification != null)
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            return notification;
        }

        public async Task<bool> Delete(Guid Id)
        {
            Notification notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == Id);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Notification>> Get()
        {
            return await _context.Notifications.ToListAsync();
        }
        public async Task<List<NotificationViewModel>> GetUnReadNotification(string userId)
        {
            if (userId == null) { return null; }
            var x = await _context.Notifications.Where(x => x.UserId == userId && x.IsRead == false).ToListAsync();
            var notificationsNR = new List<NotificationViewModel>();
            foreach(Notification c in x)
            {
                notificationsNR.Add(new NotificationViewModel { 
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    IsRead = c.IsRead,
                    UserId = c.UserId,
                    TeamId = c.TeamId
                });
            }
            return notificationsNR;
        }
        public async Task<List<NotificationViewModel>> GetReadNotification(string userId)
        {
            var x = await _context.Notifications.Where(x => x.UserId == userId && x.IsRead == true).ToListAsync();
            var notificationsR = new List<NotificationViewModel>();
            foreach (Notification c in x)
            {
                notificationsR.Add(new NotificationViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    IsRead = c.IsRead,
                    UserId = c.UserId,
                    TeamId = c.TeamId
                });
            }
            return notificationsR;
        }
        public async Task<Notification> Get(Guid Id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Notification> Update(Notification notification)
        {
            var categoryChanges = _context.Notifications.Attach(notification);
            categoryChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> NotificationExists(string userId, Guid teamId)
        {
            return await _context.Notifications.AnyAsync(n => n.UserId == userId && n.TeamId == teamId);
        }

        public async Task<string> InviteForJoin(string teamName, string username)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(username));
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Name.Equals(teamName));
            if (user == null)
            {
                return "False:' " + username + " ' doesn't exist or was deleted!";
            }
            else if (await NotificationExists(user.Id, team.Id))
            {
                return "False:You have already invited ' " + username + " '!";
            }
            else if (team == null)
            {
                return "False:You don't have a team!";
            }
            else if (user.TeamId != null)
            {
                if(user.TeamId == team.Id)
                {
                    return "False:User ' "+username+" '  is already part of your team!";
                }
                return "False:User '"+username+" ' is already part of another team!";
            }
            else
            {
                Notification playerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Name = "You have been invited in team : " + teamName,
                    IsRead = false,
                    Type = "TeamInvite",
                    UserId = user.Id,
                    TeamId = team.Id
                };
                await Add(playerNotification);
                return "True:You have invited ' " + username + " ' in your team!";
            }
        }

        public async Task<bool> InviteForMatch(string teamId,string teamIdSender)
        {
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(teamId));
            Team teamSender = await _context.Teams.FirstOrDefaultAsync(x => x.Id == Guid.Parse(teamIdSender));
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == team.TeamLeaderId.ToString());

            if (user != null)
            {
                Notification playerNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    Name = "You have been invited for a match by team: " + teamSender.Name,
                    IsRead = false,
                    Type = "MatchInvite",
                    UserId = user.Id,
                    TeamId = team.Id
                };
                await Add(playerNotification);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> JoinNotificationIsRead(string notificationId)
        {
            if(notificationId != null)
            {
                Notification notification =  await _context.Notifications.FirstOrDefaultAsync(x => x.Id == Guid.Parse(notificationId));
                if (notification != null && !notification.IsRead)
                {
                    notification.IsRead = true;
                    _context.Notifications.Update(notification);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public async Task<string> NotificationTypeCheck(string notificationId)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == Guid.Parse(notificationId));
            return notification.Type;
        }
    }
}
