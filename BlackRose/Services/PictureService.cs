using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackRose.Domain;

namespace BlackRose.Services
{
    public class PictureService : IPictureService

    {
       public readonly List<Picture> __pictures;

        public PictureService()
        {
            __pictures = new List<Picture>();
            for (var i = 0; i < 5; i++)
            {
                __pictures.Add(new Picture
                {
                    Id = Guid.NewGuid(),
                    Description = $"Description{i}"
                });
            }
        }

        public List<Picture> GetPictures()
        {
            return __pictures;
        }
        public Picture GetPictureById(Guid pictureId)
        {
            return __pictures.SingleOrDefault(x => x.Id == pictureId);
        }

      
    }
}
