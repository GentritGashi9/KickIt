using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SportsApp.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        [Required]
        public int MaxCapacity { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        [Required]
        public int MinCapacity { get; set; }

        public ICollection<SportField> SportFields {get;set;}

        public ICollection<Team> Teams { get; set; }
    }
}