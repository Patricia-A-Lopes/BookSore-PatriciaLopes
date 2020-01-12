using BookStore.Model;
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
    public partial class BookHomeView : ContentPage
    {
        BooksViewModel vm;
        public BookHomeView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            vm = new BooksViewModel();
            vm.Navigation = Navigation;
            this.BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await vm.BooksAsync();

            }
            catch
            {

            }

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            vm.OnFilterSwitch(e.Value);
        }
    }
}