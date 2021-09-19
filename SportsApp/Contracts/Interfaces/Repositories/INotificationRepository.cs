using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsApp.Models;
using SportsApp.ViewModels;

namespace SportsApp.Contracts.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> Get(Guid Id);
        Task<List<Notification>> Get();
        Task<Notification> Add(Notification fieldOwner);
        Task<Notification> Update(Notification fieldOwnerChanges);
        Task<bool> Delete(Guid Id);
        Task<bool> NotificationExists(string userId, Guid teamId);
        Task<string> InviteForJoin(string teamName,string username);
        Task<bool> InviteForMatch(string teamId,string teamIdSender);
        Task<List<NotificationViewModel>> GetUnReadNotification(string userId);
        Task<List<NotificationViewModel>> GetReadNotification(string userId);
        Task<bool> JoinNotificationIsRead(string notificationId);
        Task<string> NotificationTypeCheck(string notificationId);

    }
}