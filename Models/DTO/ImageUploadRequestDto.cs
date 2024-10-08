using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Models.DTO
{
    public class ImageUploadRequestDto
    {
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}