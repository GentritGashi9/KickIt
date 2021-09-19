using System;
using System.ComponentModel.DataAnnotations;
using SportsApp.Data;

namespace SportsApp.Models
{
    public class FieldOwner
    {
        public Guid Id { get; set; }

        [Required]
        public string BuissnessName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Required]
        public string ContactNr { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
