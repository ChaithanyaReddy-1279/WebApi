using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Models.Domain;

namespace MyWebApi.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public required RegionDto Region { get; set; }
        public required DifficultyDto Difficulty { get; set; }
        
    }
}