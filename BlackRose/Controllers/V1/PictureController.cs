using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BlackRose.Domain;
using System.IdentityModel.Tokens.Jwt;
using BlackRose.Contracts.V1;
using BlackRose.Services;
using BlackRose.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using BlackRose.Data;
using System.Collections.Generic;

namespace ImageUploadDemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class PictureController : ControllerBase
    {
        public static IHostingEnvironment _environment;
        public UserManager<IdentityUser> _userManager;
        private IPictureService _pictureService;
        public PictureController(IHostingEnvironment environment, UserManager<IdentityUser> userManager, IPictureService pictureService)
        {
            _environment = environment;
            _userManager = userManager;
            _pictureService = pictureService;
        }
        public class FIleUploadAPI
        {
            public string Description { get; set; }
            public string Tags{ get; set; }
            public IFormFile files { get; set; }
        }


        public async Task<Picture> AddPicture(FIleUploadAPI pic, string filePath)
        {

            string userId = User.Claims.First(c => c.Type == "id").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string CurrentuserName = user.UserName;
            Guid newId = Guid.NewGuid();
            var TimeNow = DateTime.UtcNow;

            var model = new Picture
            {
                UserName = CurrentuserName,
                Id = newId,
                ImagePath = filePath,
                Description = pic.Description,
                Tags = pic.Tags,
                Time=TimeNow

            };
            await _pictureService.CreatePictureAsync(model);
            return model;

        }
        [HttpGet(ApiRoutes.Pictures.GetAll)]
        public async Task<List<Picture>> GetAll([FromQuery] string tag)
        {
            if (tag != null)
            {
                return await _pictureService.GetPictureByTagAsync(tag);
            }
            return await _pictureService.GetPicturesAsync();
        }
        
        [HttpPost(ApiRoutes.Pictures.Delete)]
        public async Task<bool> DeleteGuid ([FromBody]Guid pictureId)
        {
            return await _pictureService.DeletePictureAsync(pictureId);
        }
        [HttpPost(ApiRoutes.Pictures.Create)]
        public async Task<JsonResult> Post([FromForm]FIleUploadAPI files)
        {

            if (files.files.Length > 0)
            {
                try
                {
                    string uploadsPath = _environment.WebRootPath + "/uploads/";
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }
                    using (FileStream filestream = System.IO.File.Create(uploadsPath + files.files.FileName))
                    {
                        files.files.CopyTo(filestream);
                        filestream.Flush();
                        string filePath = "/uploads/" + files.files.FileName;
                        var pic=await AddPicture(files, filePath);
                        return new JsonResult(pic);

                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex);
                }
            }
            else
            {
                return new JsonResult("Unsuccesful");
            }

        }


    }
}