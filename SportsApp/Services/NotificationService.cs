using SportsApp.Contracts.Interfaces.Repositories;
using SportsApp.Contracts.Interfaces.Services;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Services
{
    public class NotificationService : INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        
        public NotificationService(INotificationRepository categoryRepository)
        {
            _notificationRepository= categoryRepository;
        }
       
        public Task<Notification> Add(Notification category)
        {
            return _notificationRepository.Add(category);
        }

        public Task<bool> Delete(Guid Id)
        {
           return _notificationRepository.Delete(Id);
        }

        public Task<List<Notification>> Get()
        {
            return _notificationRepository.Get();
        }
        public async Task<List<NotificationViewModel>> GetUnReadNotification(string userId)
        {
            return await _notificationRepository.GetUnReadNotification(userId);
        }
        public async Task<List<NotificationViewModel>> GetReadNotification(string userId)
        {
            return await _notificationRepository.GetReadNotification(userId);
        }
        public Task<Notification> Get(Guid Id)
        {
            return _notificationRepository.Get(Id);
        }

        public Task<Notification> Update(Notification category)
        {
            return _notificationRepository.Update(category);
        }

        public Task<bool> NotificationExists(string userId, Guid teamId)
        {
            return _notificationRepository.NotificationExists(userId, teamId);
        }
    }
}
