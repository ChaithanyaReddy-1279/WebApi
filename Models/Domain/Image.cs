using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApi.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped] // This should be okay if you don't want this to be included in the DB
        public IFormFile? File { get; set; }

        public required string FileName { get; set; }

        public string? FileDescription { get; set; }

        public required string FileExtension { get; set; }

        public long FileSizeInBytes { get; set; }

        public required string FilePath { get; set; }
    }
}