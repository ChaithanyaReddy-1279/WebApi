using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class DifficultyDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        
    }
}