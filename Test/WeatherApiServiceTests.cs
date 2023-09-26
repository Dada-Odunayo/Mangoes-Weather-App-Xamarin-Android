using Mangoes;
using NUnit.Framework;
using static Android.Provider.Contacts.Intents;

namespace Test
{
    [TestFixture]
    public class WeatherApiServiceTests
    {
        [Test]
        public async Task DownloadWeatherIconAsync_ValidIconCode_ReturnsBitmap()
        {            
            var httpClient = new HttpClient();
            var weatherApiService = new WeatherApiService(httpClient);
            string validIconCode = "04n";
            
            var bitmap = await weatherApiService.DownloadWeatherIconAsync(validIconCode);
            
            Assert.IsNotNull(bitmap);
            Assert.IsTrue(bitmap.Width > 0);
            Assert.IsTrue(bitmap.Height > 0);
        }

        [Test]
        public async Task GetWeatherDataAsync_ValidCity_ReturnsWeatherData()
        {            
            var httpClient = new HttpClient();
            var weatherApiService = new WeatherApiService(httpClient);
            string validCity = "London";
            
            var weatherData = await weatherApiService.GetWeatherDataAsync(validCity);
            
            Assert.IsNotNull(weatherData);          
        }

      
    }

}