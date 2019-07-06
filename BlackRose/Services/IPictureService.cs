using System;
using System.Collections.Generic;
using BlackRose.Domain;

namespace BlackRose.Services
{
    public interface IPictureService
    {
        List<Picture> GetPictures();

        Picture GetPictureById(Guid pictureId);
    }
}