using AlphaImgs.Services.Images;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaImgs.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleImages(this IServiceCollection services, IConfiguration googleImagesConfiguration)
        {
            services.AddHttpClient<IImagesService, GoogleImagesService>();
            services.Configure<GoogleImagesServiceOptions>(googleImagesConfiguration);
            return services;
        }
    }
}