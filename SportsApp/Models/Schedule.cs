using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Reserved { get; set; }
        public Guid SportFieldId { get; set; }
        public SportField SportField { get; set; }
        public Guid? MatchesId { get; set; }
        public Matches Matches {get;set;}
    }
}
