using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [MaxLength(100, ErrorMessage = "Name cannot exceed maximum of 100 characters")]
        public required string Name { get; set; }
        [MaxLength(1000, ErrorMessage = "Description cannot exceed maximum of 1000 characters")]
        public required string Description { get; set; }
        [Range(0, 50, ErrorMessage = "Length must be between 0 and 50 km")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public required RegionDto Region { get; set; }
        public required DifficultyDto Difficulty { get; set; }
    }
}