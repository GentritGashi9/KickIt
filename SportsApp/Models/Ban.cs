using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Models
{
    public class Ban
    {
        public Guid Id { get; set; }
        public string IsBanned { get; set; }

        public Guid SportsFieldId { get; set; }
        public virtual SportField SportField { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
