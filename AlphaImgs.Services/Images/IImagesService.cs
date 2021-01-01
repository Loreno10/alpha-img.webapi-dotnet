using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaImgs.Services.Images
{
    public interface IImagesService
    {
        Task<IReadOnlyCollection<Image>> GetImages(string searchTerm, int index,
            Func<IEnumerable<Image>, IEnumerable<Image>>? manipulationFunction, CancellationToken ct);
    }
}