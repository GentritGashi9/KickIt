using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.Models
{
    public class SportField
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Required]
        public string ContactNr { get; set; }

        [Required]
        public bool IsApproved { get; set; } = false;
        public int ViewsCount { get; set; } = 0;
        public string MainPicture { get; set; }
        //Geo Cordinates
        public double SportFieldGeoLocationLat { get; set; }
        public double SportFieldGeoLocationLong { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<SportFieldPictures> Pictures { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string WorkDaysE { get; set; }
    }
}