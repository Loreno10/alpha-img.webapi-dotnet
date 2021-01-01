using System.Collections.Generic;
using AlphaImgs.Services.Images;

namespace AlphaImgs.WebAPI.Responses.Images
{
    public record ImagesList(IEnumerable<Image> Data);
}