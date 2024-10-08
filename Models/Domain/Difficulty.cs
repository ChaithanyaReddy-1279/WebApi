using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.Domain
{
    public class Difficulty
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}