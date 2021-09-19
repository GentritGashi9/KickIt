using System;

namespace SportsApp.Models
{
    public class SportFieldPictures
    {
        public Guid  Id { get; set; }

        public string Path { get; set; }

        public SportField SportField { get; set; }

        public Guid SportFieldId { get; set; }

    }
}
