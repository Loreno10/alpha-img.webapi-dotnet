using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AlphaImgs.Services.Extensions;
using Microsoft.Extensions.Options;

namespace AlphaImgs.Services.Images
{
    public class GoogleImagesService : IImagesService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleImagesServiceOptions _options;

        public GoogleImagesService(HttpClient httpClient, IOptions<GoogleImagesServiceOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            _options = options.Value;
        }

        public async Task<IReadOnlyCollection<Image>> GetImages(string searchTerm, int index, Func<IEnumerable<Image>, IEnumerable<Image>>? manipulationFunction, CancellationToken ct)
        {
            var urls = GetUrls(searchTerm, index);
            
            var tasks = urls.Select(url => _httpClient.GetAsync(url, ct)).ToList();
            await Task.WhenAll(tasks);

            var result = new List<Image>();
            foreach (var response in tasks)
            {
                await using var stream = await response.Result.Content.ReadAsStreamAsync(ct);
                var data = await JsonSerializer.DeserializeAsync<ApiResponse>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web), ct);
                result.AddRange(Map(data!));
            }

            return new ReadOnlyCollection<Image>(manipulationFunction.Invoke(result).ToArray());
        }

        private IEnumerable<Uri> GetUrls(string searchTerm, int index)
        {
            var index1 = ((index - 1) * 20 + 1).ToString();
            var index2 = ((index - 1) * 20 + 11).ToString();
            
            var url = new Uri(_options.BaseUrl)
                .AddQuery("cx", _options.Cx)
                .AddQuery("q", searchTerm)
                .AddQuery("key", _options.ApiKey)
                .AddQuery("filter", "1")
                .AddQuery("imgColorType", "trans")
                .AddQuery("searchType", "image");

            return new[] {url.AddQuery("start", index1), url.AddQuery("start", index2)};
        }

        private static IEnumerable<Image> Map(ApiResponse rawData)
        {
            var results = rawData.Items.Select(item => 
                new Image(item.Title, item.Link, item.Image.ThumbnailLink, item.Image.Height, item.Image.Width)).ToList();

            return results.AsReadOnly();
        }

        private record ApiResponse(IReadOnlyCollection<ApiResponseItem> Items);
        
        private record ApiResponseItem(string Title, string Link, ApiResponseItemDetails Image);

        private record ApiResponseItemDetails(int Height, int Width, string ThumbnailLink);
    }
}