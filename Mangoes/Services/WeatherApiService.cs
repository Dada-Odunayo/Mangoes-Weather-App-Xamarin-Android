using Android.Graphics;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mangoes
{
    public class WeatherApiService : IWeatherApiService
    {        
        private readonly HttpClient _httpClient;
        public WeatherApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://api.openweathermap.org/");
        }

        public async Task<Android.Graphics.Bitmap> DownloadWeatherIconAsync(string iconCode)
        {            
            try
            {
                byte[] iconBytes = await _httpClient.GetByteArrayAsync($"img/w/{iconCode}.png");            
                if (iconBytes != null && iconBytes.Length > 0)
                {                    
                    Bitmap iconBitmap = BitmapFactory.DecodeByteArray(iconBytes, 0, iconBytes.Length);
                    return iconBitmap;
                }
                else
                {                                        
                    return null;
                }                
            }
            catch (Exception ex)
            {
                Log.Error("Image Exception", ex.Message);
                return null;
            }                       
        }

       public async Task<WeatherResponseModel> GetWeatherDataAsync(string city)
        {
            try
            {                
                var response = await _httpClient.GetAsync($"data/2.5/weather?q={Uri.EscapeDataString(city)}&apiKey={Uri.EscapeDataString("1c47f9c78f7420e7e598df755946dba9")}&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherResponseModel>(json);
                    return weatherData;
                }
                else
                {                    
                    return null;
                }
            }
            catch (Exception ex)
            {                
                Log.Error("ApiException", ex.Message);
                return null;
            }
        }
    }
    
}