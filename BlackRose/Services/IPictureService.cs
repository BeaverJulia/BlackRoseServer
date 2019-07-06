using BlackRose.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackRose.Services
{
   public interface IPictureService
    {
        List<Picture> GetPictures();

        Picture GetPictureById(Guid pictureId);
    }
}
