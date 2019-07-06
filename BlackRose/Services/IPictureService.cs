using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlackRose.Domain;

namespace BlackRose.Services
{
    public interface IPictureService
    {
        Task<List<Picture>> GetPicturesAsync();

        Task<Picture> GetPictureByIdAsync(Guid pictureId);

        Task<bool> DeletePictureAsync(Guid pictureId);
        Task<bool> CreatePictureAsync(Picture picture);
    }
}