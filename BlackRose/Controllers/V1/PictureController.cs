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
using BlackRose.Data
namespace ImageUploadDemo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class PictureController : ControllerBase
    {
        public string PathImage;
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


        public async Task AddPicture(FIleUploadAPI pic)
        {

            string userId = User.Claims.First(c => c.Type == "id").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string CurrentuserName = user.UserName;
            Guid.TryParse(user.Id, out Guid CurrentId);
            
            var model = new Picture
            {
                UserName = CurrentuserName,
                Id = CurrentId,
                ImagePath = PathImage,
                Description = pic.Description,
                Tags = pic.Tags

            };

            DataContext
        }
        [HttpPost(ApiRoutes.Pictures.Create)]
        public async Task<string> Post([FromForm]FIleUploadAPI files)
        {

            if (files.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + files.files.FileName))
                    {
                        files.files.CopyTo(filestream);
                        filestream.Flush();
                        PathImage = "\\uploads\\" + files.files.FileName;
                        await AddPicture(files);
                        return (PathImage);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }

        }


    }
}