using Bloggie.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImagesRepository imagesRepository;

        public ImagesController(IImagesRepository imagesRepository)
        {
            this.imagesRepository = imagesRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
          var imageUrl =  await imagesRepository.UploadAsynce(file);
            if(imageUrl == null)
            {
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }
            return Json(new { link = imageUrl });
        }
    }
}
