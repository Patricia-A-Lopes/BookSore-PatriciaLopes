using BookStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookStore.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BookDetailedView : ContentPage
	{
        BooksDetailedViewModel vm;
        private string volumeId;
        public BookDetailedView(string volumeid)
        {

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            vm = new BooksDetailedViewModel();
            vm.navigation = Navigation;
            this.BindingContext = vm;

            volumeId = volumeid;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await vm.BooksDetailAsync(volumeId);

            }
            catch
            {

            }

        }
    }
}