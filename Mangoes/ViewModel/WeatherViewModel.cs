using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mangoes
{
    public class WeatherViewModel: ViewModel
    {
        private readonly IWeatherApiService _weatherApiService;
        

        public WeatherViewModel(IWeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;//new WeatherApiService(new HttpClient());
        }

        public async Task<WeatherResponseModel> GetWeatherDataAsync(string city)
        {                
            return await _weatherApiService.GetWeatherDataAsync(city);
        }

        public async Task<Android.Graphics.Bitmap> DownloadWeatherIconAsync(string iconCode)
        {
            return await _weatherApiService.DownloadWeatherIconAsync(iconCode);
        }
    }
}