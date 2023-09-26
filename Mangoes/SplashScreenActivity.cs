using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangoes
{    
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme")]
    public class SplashScreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.splash_screen);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await Task.Delay(1500);
            Intent intent = new Intent(this, typeof(HomeActivity));
            StartActivity(intent);
        }
    }
}