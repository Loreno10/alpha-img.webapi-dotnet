using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlphaImgs.Services.Extensions;
using AlphaImgs.Services.Images;
using AlphaImgs.WebAPI.Responses.Images;
using Microsoft.AspNetCore.Mvc;

namespace AlphaImgs.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet]
        public async Task<ImagesList> Get(string searchTerm, int index, CancellationToken ct)
        {
            var images = (await _imagesService.GetImages(
                    searchTerm, 
                    index, 
                    imgs => imgs.OrderByDescending(n => n.Width), 
                    ct))
                .RemoveNonPng();
            
            return new ImagesList(images);
        }
    }
}