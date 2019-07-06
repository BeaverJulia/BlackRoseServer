using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackRose.Contracts.Request;
using BlackRose.Contracts.V1;
using BlackRose.Contracts.V1.Responses;
using BlackRose.Domain;
using BlackRose.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlackRose.Controllers
{
    public class PictureController : Controller
    {
        private  IPictureService _pictureService;
        
        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }
        [HttpGet( ApiRoutes.Pictures.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_pictureService.GetPictures());
        }
        [HttpGet(ApiRoutes.Pictures.Get)]
        public IActionResult Get([FromRoute] Guid pictureId)
        {
            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(picture);
            }
        }

        [HttpPost(ApiRoutes.Pictures.Create)]
        public IActionResult Create([FromBody] CreatePictureRequest pictureRequest)
        {
            var picture = new Picture { Id = pictureRequest.Id };

            if (picture.Id != Guid.Empty)
                picture.Id = Guid.NewGuid();
            _pictureService.GetPictures().Add(picture);

            var baseurl=$"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseurl + "/" + ApiRoutes.Pictures.Get.Replace("{pictureId}", picture.Id.ToString());

            var response = new PictureResponse { Id = picture.Id };
            return Created(locationUrl, response);
        }
    }
}