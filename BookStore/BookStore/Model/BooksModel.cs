using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BookStore.Model
{

    public class BooksModel
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string kind { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public Volumeinfo volumeInfo { get; set; } 
        public Saleinfo saleInfo { get; set; }
        public bool Favorite { get; set; } = false;
    }

    public class Volumeinfo
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string[] authors { get; set; }
        public string publisher { get; set; }
        public string publishedDate { get; set; }
        public string description { get; set; }
        public Imagelinks imageLinks { get; set; } 
        
    }

 
    public class Imagelinks
    {
        public string smallThumbnail { get; set; }
        public string thumbnail { get; set; }
        public ImageSource image
        {
            get { return ImageSource.FromUri(new Uri(thumbnail)); }
            set { image = value; }
        }
    }

  

    public class Saleinfo
    {
        public string country { get; set; }
        public string saleability { get; set; }
        public bool isEbook { get; set; }
        public Listprice listPrice { get; set; }
        public Retailprice retailPrice { get; set; }
        public string buyLink { get; set; }
    }

    public class Listprice
    {
        public float amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class Retailprice
    {
        public float amount { get; set; }
        public string currencyCode { get; set; }
    }

  
}

