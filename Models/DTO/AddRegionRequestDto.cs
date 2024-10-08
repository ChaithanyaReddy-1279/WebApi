using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class AddRegionRequestDto
    {
        [MinLength(3, ErrorMessage = "Code length must be a minimum of 3 characters")]  
        [MaxLength(3, ErrorMessage = "Code length must be a maximum of 3 characters")]
        public required string Code { get; set; }
        [MaxLength(100, ErrorMessage = "Name cannot exceed maximum of 100 characters")]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}