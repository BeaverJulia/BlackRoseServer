using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackRose.Data;
using BlackRose.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlackRose.Services
{
    public class PictureService : IPictureService

    {
        public readonly List<Picture> __pictures;
        private readonly DataContext _dataContext;

        public PictureService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
      

        public async Task <List<Picture>> GetPicturesAsync()
        {
            return await _dataContext.pictureContext.ToListAsync();
        }

        public async Task <Picture> GetPictureByIdAsync(Guid pictureId)
        {
            return await _dataContext.pictureContext.SingleOrDefaultAsync(x => x.Id == pictureId);
              
        }

        public async Task <bool> CreatePictureAsync (Picture picture)
        {
            await _dataContext.pictureContext.AddAsync(picture);
            await _dataContext.SaveChangesAsync();
            return (true);


        }
        public async Task <bool> DeletePictureAsync (Guid pictureId)
            {
            var picture = await GetPictureByIdAsync(pictureId);
            _dataContext.pictureContext.Remove(picture);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}