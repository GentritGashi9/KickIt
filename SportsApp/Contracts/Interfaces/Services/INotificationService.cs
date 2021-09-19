using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsApp.Contracts.Interfaces.Services
{
    public interface INotificationService
    {
        Task<Notification> Get(Guid Id);
        Task<List<Notification>> Get();
        Task<Notification> Add(Notification fieldOwner);
        Task<Notification> Update(Notification fieldOwnerChanges);
        Task<List<NotificationViewModel>> GetUnReadNotification(string userId);
        Task<List<NotificationViewModel>> GetReadNotification(string userId);
        Task<bool> Delete(Guid Id);
        Task<bool> NotificationExists(string userId, Guid teamId);
    }
}
