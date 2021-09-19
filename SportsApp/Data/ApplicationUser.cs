using Microsoft.AspNetCore.Identity;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(4)]
        [Column(TypeName = "Varchar(30)")]
        public string Name { get; set; }

        [Required]
        [MinLength(4)]
        [Column(TypeName = "Varchar(30)")]
        public string Surname { get; set; }

        public Cities Location { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Role { get; set; }

        public string ProfileImg { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsBanned {get; set;} = false;

        public Guid? TeamId { get; set; }
        
        public Team Team { get; set; }

        public ICollection<GameRoomChat> Room { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<Notification> Notifications { get; set; }

        //public DateTimeOffset BanEnd{get; set;}
    }
}

