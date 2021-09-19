using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.ViewModels
{
    public class TeamViewModel
    {
        public Guid Id { get; set; } //Team ID

        [Required(ErrorMessage = "Please write a correct Name.")]
        [MinLength(4, ErrorMessage = "Please enter at least 4 caracters.")]
        [MaxLength(12, ErrorMessage = "This name is too long! (max 12 caracters)")]
        public string Name { get; set; }

        public Cities City { get; set; }

        public string CityName { get; set; }

        public List<Category> Categories { get; set; }

        public string CategoryName { get; set; }

        public Guid CategoryId { get; set; }

        public int MyProperty { get; set; }

        public string TeamLeaderId { get; set; }

        public string TeamLeaderName { get; set; }

        public string PlayerName { get; set; }

        public Guid PlayerId { get; set; }

        public bool isPrivate { get; set; }

        public int NumberOfPlayers { get; set; }

        public int MaxPlayers { get; set; }
    }
}
