using System.Collections.Generic;
using System.Linq;
using AlphaImgs.Services.Images;

namespace AlphaImgs.Services.Extensions
{
    public static class ImageExtensions
    {
        public static IEnumerable<Image> RemoveNonPng(this IEnumerable<Image> images) =>
            images.Where(n => !n.ImageUrl.EndsWith("png") || !n.ImageUrl.EndsWith("PNG"));
    }
}