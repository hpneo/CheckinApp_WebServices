using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.IO;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace CheckinApp_Webservices
{
	[Activity (Label = "CheckinApp Web Services", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{
		private ListView listViewMovies;
		private EditText editTextNewMovie;
		private Button buttonAddMovie;
		private ArrayAdapter adapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			editTextNewMovie = FindViewById<EditText> (Resource.Id.editTextNewMovie);
			buttonAddMovie = FindViewById<Button> (Resource.Id.buttonAddMovie);

			listViewMovies = FindViewById<ListView> (Resource.Id.listViewMovies);
			adapter = new ArrayAdapter (this, Resource.Layout.MovieItem, new string[] { });

			listViewMovies.Adapter = adapter;

			buttonAddMovie.Click += async delegate(object sender, EventArgs e) {
				string newMovie = editTextNewMovie.Text.Trim ();
				TMDB api = new TMDB();

				if (newMovie != "") {
					Task<object> resultsTask = api.searchMovies(newMovie);

					JObject results = await resultsTask as JObject;

					JArray moviesArray = (JArray)results["results"];

					foreach(var movieJSON in moviesArray) {
						adapter.Add(movieJSON["title"].ToString());
					}
				}
			};
		}
	}
}


