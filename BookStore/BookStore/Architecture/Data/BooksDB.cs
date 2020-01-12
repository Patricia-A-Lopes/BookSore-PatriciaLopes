using BookStore.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Architecture.Data
{
   public class BooksDB
    {
        readonly SQLiteAsyncConnection _database;

        /// <summary>
        /// creates an connection and an table, if doest exist
        /// </summary>
        /// <param name="dbPath"></param>
        public BooksDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            
            _database.CreateTableAsync<BookModel>().Wait();
        }

        /// <summary>
        /// Gets the lis of all books
        /// </summary>
        public Task<List<BookModel>> GetBooksAsync()
        {
            return _database.Table<BookModel>().ToListAsync();
        }

        /// <summary>
        /// Gets the book, if exist
        /// </summary>
        public Task<BookModel> GetBookAsync(string id)
        {
            return _database.Table<BookModel>()
                            .Where(i => i.id == id)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// insert/updates the book
        /// </summary>
        public Task<int> SaveBookAsync(BookModel book)
        {
          var exists=_database.Table<BookModel>()
                            .Where(i => i.id == book.id)
                            .FirstOrDefaultAsync();
            if (exists.Result!=null)
            {
                return _database.UpdateAsync(book);
            }
            else
            {
                return _database.InsertAsync(book);
            }
        }

        /// <summary>
        /// deletes the book
        /// </summary>
        public Task<int> DeleteBookAsync(BookModel book)
        {
            return _database.DeleteAsync(book);
        }
    }
}

