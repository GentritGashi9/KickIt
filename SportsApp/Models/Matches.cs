using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Models
{
    public class Matches
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Team1Id { get; set; }
        public string Team2Id { get; set; }
        public string SportFieldId { get; set; }
        public Schedule Schedule { get; set; }
        public bool IsAccepted { get; set; } = false;
    }
}
