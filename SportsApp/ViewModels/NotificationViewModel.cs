using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.ViewModels
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
