using Microsoft.AspNetCore.Http;
using SportsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsApp.Views.SportField
{
    public class SportFieldViewModel
    {
        [Required(ErrorMessage = "Please write a correct Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please write a correct Addrese.")]
        public string Address { get; set; }

        [Phone]
        [Required(ErrorMessage = "Please write a correct phone number.")]
        public string ContactNr { get; set; }

        public List<IFormFile> Pictures { get; set; }

        [Required(ErrorMessage = "Please select a location in the map.")]
        public double SportFieldGeoLocationLat { get; set; }

        [Required(ErrorMessage = "Please select a location in the map.")]
        public double SportFieldGeoLocationLong { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [Required(ErrorMessage = "Please choose the Working Hours")]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Please choose the Working Days")]
        public List<Days> Workingdays { get; set; }
    
        public List<Category> Categories { get; set; }
        [Required(ErrorMessage = "Please choose the Category")]
        public Guid CategoryId { get; set; }

    }
}
