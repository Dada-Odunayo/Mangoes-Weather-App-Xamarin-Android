using Android.Graphics;
using Refit;
using System.Threading.Tasks;

namespace Mangoes
{
    public interface IWeatherApiService
    {
        [Get("/weather")]
        public Task<WeatherResponseModel> GetWeatherDataAsync([Query("city")]string city);

        public Task<Bitmap> DownloadWeatherIconAsync(string iconCode);
    }
}