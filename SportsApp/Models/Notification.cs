using System;
using SportsApp.Data;

namespace SportsApp.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid TeamId { get; set; }
    }
}