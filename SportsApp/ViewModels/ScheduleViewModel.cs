using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.ViewModels
{
    public class ScheduleViewModel
    {
        public Guid MatchId { get; set; }
        public string MatchName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
