using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class LoginRequestDto
    {
        [DataType(DataType.Password)]
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}