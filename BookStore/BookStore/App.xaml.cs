using BookStore.Architecture.Data;
using BookStore.View;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace BookStore
{
	public partial class App : Application
	{
        static BooksDB db;
        public static BooksDB DB
        {
            get
            {
                if (db == null)
                {
                    db = new BooksDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BookStore.db3"));
                }
                return db;
            }
        }

        public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new BookHomeView());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
