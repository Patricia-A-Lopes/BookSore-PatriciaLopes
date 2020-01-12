using BookStore.Architecture;
using BookStore.Model;
using BookStore.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BookStore.ViewModel
{

    public class BooksViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }


        public BooksViewModel()
        {
            bookTapCommand = new Command(OnBookTappedAsync);

        }

        #region Bindings

        ICommand bookTapCommand;
        public ICommand BookTapCommand
        {
            get { return bookTapCommand; }
            set
            {
                bookTapCommand = value;
                OnPropertyChanged("BookTapCommand");
            }
        }

        private BooksModel books;
        BooksModel Books
        {
            get { return books; }
            set { books = value; }
        }

        private List<Item> book;
        public List<Item> Book
        {
            get { return book; }
            set
            {
                book = value;
                OnPropertyChanged("Book");
            }
        }
        private List<Item> favoriteBook;
        public List<Item> FavoriteBook
        {
            get { return favoriteBook; }
            set
            {
                favoriteBook = value;
                OnPropertyChanged("FavoriteBook");
            }
        }
        private ObservableCollection<BookPair> allBooks;

        public ObservableCollection<BookPair> AllBooks
        {
            get { return allBooks; }
            set
            {
                allBooks = value;
                OnPropertyChanged("AllBooks");
            }
        }
        private ObservableCollection<BookPair> favoritePairedBooks;

        public ObservableCollection<BookPair> FavoritePairedBooks
        {
            get { return favoritePairedBooks; }
            set
            {
                favoritePairedBooks = value;
                OnPropertyChanged("FavoritePairedBooks");
            }
        }

        private ObservableCollection<BookPair> bookList;

        public ObservableCollection<BookPair> BookList
        {
            get { return bookList; }
            set
            {
                bookList = value;
                OnPropertyChanged("BookList");
            }
        }

        private bool isListVisible = true;
        public bool IsListVisible
        {
            get { return isListVisible; }
            set
            {
                isListVisible = value;
                OnPropertyChanged("IsListVisible");
            }
        }
        private bool isLabelVisible = false;
        public bool IsLabelVisible
        {
            get { return isLabelVisible; }
            set
            {
                isLabelVisible = value;
                OnPropertyChanged("IsLabelVisible");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {

            var propertyChangedCallback = PropertyChanged;
            propertyChangedCallback?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public async Task BooksAsync()
        {
            ///GETS THE BOOKS MARKED AS FAVORITES
            var savedBooks = App.DB.GetBooksAsync();

            ///GETS BOOKS FROM API
            Books = await API.getBooksAsync();
            if (books != null)
            {
                Book = books.items;
            }

            ///will the books marked with favorite on the favorite list
            if (savedBooks.Result != null)
            {
                FavoriteBook = (from b in Book
                                join f in savedBooks.Result on b.id equals f.id
                                where f.Favorite
                                select b).ToList();
            }


            AllBooks = CreateBookPairs(Book);
            BookList = AllBooks;

        }

        /// <summary>
        /// will create an collection with an structure of a sequece of 2 books 
        /// </summary>
        private ObservableCollection<BookPair> CreateBookPairs(List<Item> List)
        {
            var pairedList = new ObservableCollection<BookPair>();
            for (int i = 0; i < List.Count; i += 2)
            {
                Item item1 = List[i];
                Item item2 = i + 1 < List.Count ? List[i + 1] : null;

                pairedList.Add(new BookPair(item1, item2));

            }
            return pairedList;

        }

        /// <summary>
        /// Navigates to the book detail 
        /// </summary>
        async void OnBookTappedAsync(object sender)
        {
            string volumeId = (string)sender;
            await Navigation.PushAsync(new BookDetailedView(volumeId));
        }


        /// <summary>
        /// Will apply the "filter" just favorites books or all 
        /// </summary>
        /// <param name="isToggled"></param>
        public void OnFilterSwitch(bool isToggled)
        {
            ///if the toggle is true, will be generate an collection of favorite books
            
            if (isToggled)
            {
                if (FavoritePairedBooks == null || FavoritePairedBooks.Count() != FavoriteBook.Count())
                {
                    if (FavoriteBook == null)
                    {
                        IsLabelVisible = true;
                        isListVisible = !IsLabelVisible;
                    }
                    else
                    {
                        isListVisible = true;
                        IsLabelVisible = !isListVisible;
                    }
                    FavoritePairedBooks = CreateBookPairs(FavoriteBook);
                }
                BookList = FavoritePairedBooks;
            }
            ///if false the main list will be set with the collection of all books 
            else
            {
                BookList = AllBooks;
            }
        }

        
        public class BookPair : Tuple<Item, Item>
        {
            public BookPair(Item item1, Item item2)
                : base(item1, item2 ?? CreateEmptyModel()) { }

            private static Item CreateEmptyModel()
            {
                return null;
            }
        }

      
    }
}
