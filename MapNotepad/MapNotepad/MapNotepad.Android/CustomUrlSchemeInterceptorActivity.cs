using Android.App;
using Android.Content;
using Android.OS;
using System;
using Android.Content.PM;


namespace MapNotepad.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
				new[] { Intent.ActionView },
				Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
				DataSchemes = new[] { "com.googleusercontent.apps.124598732874-4t34avmo2oh78qmu76tnkoeh8g489kj4" },
				DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			Helpers.AuthHelpers.AuthenticationState.Authenticator.OnPageLoading(uri);

			Finish();
		}
	}
}