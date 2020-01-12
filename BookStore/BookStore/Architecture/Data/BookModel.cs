using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Architecture.Data
{
    public class BookModel
    {
        [PrimaryKey]
        public string id { get; set; }
        public bool Favorite { get; set; } = false;

    }
}
