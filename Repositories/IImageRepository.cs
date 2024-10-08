using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Models.Domain;

namespace MyWebApi.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}