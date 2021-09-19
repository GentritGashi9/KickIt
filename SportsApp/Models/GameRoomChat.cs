using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Models
{
    public class GameRoomChat
    {
        public Guid Id { get; set; }
        public string ChatName { get; set; }
        public string ChatImg { get; set; }
        public Guid GameRoomId { get; set; }
        public GameRoom GameRoom { get; set; }
        public string AdminId { get; set; }
        public ApplicationUser Admin { get; set; }
        public ICollection<Message> Messages { get; set; } 

    }
}
