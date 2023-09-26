using Android.App;
using Android.Content;
using Android.Graphics;
using Android.InputMethodServices;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Lifecycle;
using Mangoes.Activity;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using AppCompatActivity = AndroidX.AppCompat.App.AppCompatActivity;
using Bitmap = Android.Graphics.Bitmap;

namespace Mangoes
{
    [Activity(Label = "Weather App", Theme = "@style/AppTheme")]
    public class HomeActivity : AppCompatActivity//, IViewModelStoreOwner
    {
        private EditText mSearchEditText;
        private TextView mTemp, mCity, mCountry, mDescription;
        private Button mSearchButton;
        private WeatherViewModel viewModel;
        private ImageView mWeatherIcon;
        private Button mMoreDetails,mSaveCity;
        private ProgressBar mProgressBar;
        private View overlay;        
        private ViewModelStore viewModelStore = new ViewModelStore();
        private WeatherResponseModel response;
        private Bitmap weatherIcon;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.home_screen);
            mSearchEditText = FindViewById<EditText>(Resource.Id.search_bar);
            mSearchButton = FindViewById<Button>(Resource.Id.search_button);
            mWeatherIcon = FindViewById<ImageView>(Resource.Id.weather_icon);
            mTemp = FindViewById<TextView>(Resource.Id.temp);
            mCity = FindViewById<TextView>(Resource.Id.city_name);
            mCountry = FindViewById<TextView>(Resource.Id.country);
            mDescription = FindViewById<TextView>(Resource.Id.description);
            mMoreDetails = FindViewById<Button>(Resource.Id.more_details);
            mSaveCity = FindViewById<Button>(Resource.Id.save_city);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            overlay = FindViewById<View>(Resource.Id.dark_overlay);

            GetFavoriteCity();            
            viewModel = new WeatherViewModel(new WeatherApiService(new System.Net.Http.HttpClient()));
            mSearchButton.Click += delegate
            {
                FetchWeatherDetails();
            };
            mMoreDetails.Click += delegate
            {
                NavigateToDetailsScreen();
            };
            mSaveCity.Click += delegate
            {
                SaveFavoriteCity(mCity.Text.Trim());
            };
        }

        public override void OnBackPressed()
        {
            //
        }

        private void SaveFavoriteCity(string city)
        {
            ISharedPreferences prefs = GetSharedPreferences("MyAppPreferences", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("FavoriteCity", city);
            editor.Apply();
        }

        private void GetFavoriteCity()
        {
            ISharedPreferences prefs = GetSharedPreferences("MyAppPreferences", FileCreationMode.Private);
            string savedCity = prefs.GetString("FavoriteCity", "");

            // If a favorite city is saved, prepopulate the city name field
            if (!string.IsNullOrEmpty(savedCity))
            {
                mSearchEditText.Text = savedCity;
            }
        }

        private void NavigateToDetailsScreen()
        {
            if(response == null)
            {
                Toast.MakeText(this,"Cannot proceed without fetching weather details",ToastLength.Long).Show();
            }

            MemoryStream stream = new MemoryStream();
            weatherIcon.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
            byte[] byteArray = stream.ToArray();
            
            Intent intent = new Intent(this, typeof(DetailsActivity));
            intent.PutExtra("response",JsonConvert.SerializeObject(response));
            intent.PutExtra("icon", byteArray);
            StartActivity(intent);
        }

     

        private async void FetchWeatherDetails()
        {
            var city = mSearchEditText.Text;
            var keyboard = (InputMethodManager)GetSystemService(Context.InputMethodService);
            keyboard.HideSoftInputFromWindow(mSearchEditText.WindowToken, 0);
            if(string.IsNullOrEmpty(city))
            {
                mSearchEditText.Error = "City cannot be empty";                
            }
            else
            {
                RunOnUiThread(() =>
                {
                    mProgressBar.Visibility = ViewStates.Visible;
                    overlay.Visibility = ViewStates.Visible;
                });
                response = await viewModel.GetWeatherDataAsync(city);
                mProgressBar.Visibility = ViewStates.Gone;
                overlay.Visibility = ViewStates.Gone;

                if(response == null)
                {                       
                        Toast.MakeText(this, "Cannot Fetch Weather Data, Try Again", ToastLength.Short).Show();
                        return;
                }
                mCity.Text = $"{response.name}";
                mCountry.Text = response.sys.country;                
                mDescription.Text = response.weather[0].main;
                mTemp.Text = $"{response.main.temp}\u00B0C";
                mMoreDetails.Visibility = ViewStates.Visible;
                mSaveCity.Visibility = ViewStates.Visible;
                string icon = response.weather[0].icon;
                weatherIcon = await viewModel.DownloadWeatherIconAsync(icon);
                if (weatherIcon == null)
                {
                    Toast.MakeText(this, "Weather Icon Download Failed", ToastLength.Short).Show();
                }
                mWeatherIcon.SetImageBitmap(weatherIcon);
                return;                                                                 
            }

        }
    }
}