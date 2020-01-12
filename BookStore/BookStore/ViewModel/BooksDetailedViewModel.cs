using BookStore.Architecture;
using BookStore.Architecture.Enum;
using BookStore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using BookStore.Architecture.Data;

namespace BookStore.ViewModel
{
    public class BooksDetailedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation navigation { get; set; }


        public BooksDetailedViewModel()
        {
            buybookCommand = new Command(BuyBookMethod);
            markFavCommand = new Command(MarkAsFavAsync);
            backCommand = new Command(BackMethod);

        }
        #region Bindings 

        ICommand buybookCommand;
        public ICommand BuyBookCommand
        {
            get { return buybookCommand; }
            set
            {
                buybookCommand = value;
                OnPropertyChanged("BuyBookCommand");
            }
        }
        ICommand markFavCommand;
        public ICommand MarkFavCommand
        {
            get { return markFavCommand; }
            set
            {
                markFavCommand = value;
                OnPropertyChanged("MarkFavCommand");
            }
        }

        ICommand backCommand;
        public ICommand BackCommand
        {
            get { return backCommand; }
            set
            {
                backCommand = value;
                OnPropertyChanged("BackCommand");
            }
        }
        private Item book;
        public Item Book
        {
            get { return book; }
            set { book = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
      
        private string titleNav;
        public string TitleNav
        {
            get { return titleNav; }
            set
            {
                titleNav = value;
                OnPropertyChanged("TitleNav");
            }
        }
        private string author;
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        private string buy;
        public string Buy
        {
            get { return buy; }
            set
            {
                buy = value;
                OnPropertyChanged("Buy");
            }
        }
        private bool isButtonVisible = false;
        public bool IsButtonVisible
        {
            get { return isButtonVisible; }
            set
            {
                isButtonVisible = value;
                OnPropertyChanged("IsButtonVisible");
            }
        }
        private string buyLink;
        BookModel isBookSaved;

        private bool isFavorite=false;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                isFavorite = value;
                OnPropertyChanged("FavoriteSource");
            }
        }


        private string favoriteSource = "unfavButton.png" ;
        public string FavoriteSource
        {
            get { return IsFavorite ? "favButton.png" : "unfavButton.png"; }
            set
            {
                favoriteSource = value;
                OnPropertyChanged("FavoriteSource");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {

            var propertyChangedCallback = PropertyChanged;
            propertyChangedCallback?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion 

        ///Call Api
        public async Task BooksDetailAsync(string volumeId)
        {
            Book = await API.getBookDetailAsync(volumeId);
            ///if result isnt null, then set values on the variables
            if (Book != null)
            {
                Image = book.volumeInfo.imageLinks.image;
                TitleNav = book.volumeInfo.title;
                Title = String.Format("Title: {0}", TitleNav);
                if (book.volumeInfo.authors != null)
                {
                    foreach (var item in book.volumeInfo.authors)
                    {
                        Author = String.Format("{0} {1},", Author, item);
                    }
                }

                Author = String.Format("Author(s): {0}", Author ?? "No Authors to display");
                Description = Regex.Replace(book.volumeInfo.description, "<.*?>", String.Empty) ?? "No description available";
                Buy = book.saleInfo.saleability;
                buyLink = book.saleInfo.buyLink;

                ///check if the book is already mark as favorite
                isBookSaved = await App.DB.GetBookAsync(Book.id);
                if (isBookSaved != null)
                {
                    IsFavorite = isBookSaved.Favorite; 
                }
                CheckBookAvailability(book.saleInfo.saleability);

            }
        }

        ///Check if the book is available, if true the buy button will be set true
        void CheckBookAvailability(string bookAvailability)
        {
            if (bookAvailability == BookAvailabilityEnum.FOR_SALE.ToString())
            {
                IsButtonVisible = true;
            }
            else
            {
                IsButtonVisible = false;
            }

        }

        ///Open the buy link i default browser
        void BuyBookMethod(object sender)
        {
            Device.OpenUri(new Uri(buyLink));


        }
        ///Mark as Fav/UnFav
        async void MarkAsFavAsync(object sender)
        {
            IsFavorite = !IsFavorite;
            Book.Favorite = IsFavorite;
            if (isBookSaved != null)
            {
                isBookSaved.Favorite = IsFavorite;
                await App.DB.SaveBookAsync(isBookSaved);

            }
            else
            {
                BookModel bm = new BookModel() { id = book.id, Favorite = IsFavorite };
                await App.DB.SaveBookAsync(bm);

            }

        }

        async void BackMethod(object sender)
        {
            await navigation.PopAsync();
        }
    }
}
