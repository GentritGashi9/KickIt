using SportsApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.Models
{
    public class SportFieldCategory
    {
        public Guid SportFieldId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Required]
        public string ContactNr { get; set; }
        public int ViewsCount { get; set; }
        public string MainPicture { get; set; }

        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }

        public string FieldOwner { get; set; }
        public string ApplicationUserId { get; set; }

        public bool IsApproved { get; set; }
        public double SportFieldGeoLocationLat { get; set; }

        public double SportFieldGeoLocationLong { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WorkDaysE { get; set; }


    }
}