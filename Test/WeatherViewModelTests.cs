using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mangoes.Tests
{
    [TestFixture]
    public class WeatherViewModelTests
    {
        
        [Test]        
        public async Task GetWeatherDataAsync_ValidCity_ReturnsWeatherData()
        {            
            var mockWeatherApiService = new Mock<IWeatherApiService>();
            mockWeatherApiService.Setup(x => x.GetWeatherDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new WeatherResponseModel());
            var weatherViewModel = new WeatherViewModel(mockWeatherApiService.Object);
            
            var weatherData = await weatherViewModel.GetWeatherDataAsync("London");
            
            Assert.IsNotNull(weatherData);            
        }      

        [Test]
        public async Task GetWeatherDataAsync_InvalidCity_ReturnsNull()
        {

            var httpClient = new HttpClient();
            var weatherApiService = new WeatherApiService(httpClient);
            string invalidCity = ".,";

            var weatherData = await weatherApiService.GetWeatherDataAsync(invalidCity);

            Assert.IsNull(weatherData);
        }
    }
}
