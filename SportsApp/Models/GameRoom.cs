using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Models
{
    public class GameRoom
    {
        public Guid Id { get; set; }
        public ICollection<Team> Teams { get; set; } 
        public string Name { get; set; }
        public string ChatImg { get; set; }
        public Guid? SportFieldId { get; set; }
        public virtual SportField SportField { get; set; }
        public ICollection<GameRoomChat> GameRoomChat { get; set; }
        public Matches Matches { get; set; }

    }
}
