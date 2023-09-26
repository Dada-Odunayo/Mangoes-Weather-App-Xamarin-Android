using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mangoes.Activity
{
    [Activity(Theme = "@style/SplashTheme")]
    public class DetailsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.details_screen);
            TextView city = FindViewById<TextView>(Resource.Id.city);
            TextView condition = FindViewById<TextView>(Resource.Id.condition);
            TextView description = FindViewById<TextView>(Resource.Id.description);
            TextView temp = FindViewById<TextView>(Resource.Id.temp);
            TextView feelsLike = FindViewById<TextView>(Resource.Id.feels_like);
            TextView maxTemp = FindViewById<TextView>(Resource.Id.max_temp);
            TextView minTemp = FindViewById<TextView>(Resource.Id.min_temp);
            TextView humidity = FindViewById<TextView>(Resource.Id.humidity);
            TextView windSpeed = FindViewById<TextView>(Resource.Id.wind_speed);
            TextView lon = FindViewById<TextView>(Resource.Id.lon);
            TextView lat = FindViewById<TextView>(Resource.Id.lat);
            ImageView weatherIcon = FindViewById<ImageView>(Resource.Id.weather_icon);

            var res = Intent.GetStringExtra("response");
            var icon = Intent.GetByteArrayExtra("icon");
            var response = JsonConvert.DeserializeObject<WeatherResponseModel>(res);
            
            city.Text = response.name;
            condition.Text += response.weather[0].main;
            description.Text += response.weather[0].description;
            temp.Text += response.main.temp + "\u00B0C";
            feelsLike.Text += response.main.feels_like + "\u00B0C";
            maxTemp.Text += response.main.temp_max+ "\u00B0C";
            minTemp.Text += response.main.temp_min + "\u00B0C";
            humidity.Text += response.main.humidity+ "%";
            windSpeed.Text += response.wind.speed + " m/s";
            lon.Text += response.coord.lon;
            lat.Text += response.coord.lat;
            if(icon != null)
            {
                Bitmap bitmap = BitmapFactory.DecodeByteArray(icon, 0, icon.Length);
                weatherIcon.SetImageBitmap(bitmap);
            }
            
        }
    }
}