using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.Models
{
    public class TeamRequests
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; } 
        public Team Team { get; set; }
        public string PlayerId { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }
}
