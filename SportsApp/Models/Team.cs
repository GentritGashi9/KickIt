using Microsoft.EntityFrameworkCore;
using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.Models
{
    public class Team
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(12)]
        public string Name { get; set; }

        public Cities City { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public string MatchId { get; set; }

        public Guid TeamLeaderId { get; set; }

        public virtual List<ApplicationUser> Players { get; set; }

        public virtual List<TeamRequests> PlayersInRequest { get; set; }

        public Guid? GameRoomId { get; set; }

        public GameRoom GameRoom { get; set; }

        public bool isPrivate { get; set; }
    }
}
