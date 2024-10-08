using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class LoginResponseDto
    {
        public required string JwtToken { get; set; }
    }
}