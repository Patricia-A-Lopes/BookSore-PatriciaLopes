using BookStore.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Architecture
{
    public class API
    {
        private static RestClient _restClientGetAll = new RestClient("https://www.googleapis.com/books/v1/volumes?q=mobile");
        private static string bookDetail = "books/v1/volumes/{0}";
        private static RestClient _rest = new RestClient("https://www.googleapis.com/");


        /// <summary>
        /// API call to get the list of all books
        /// </summary>
        public static async Task<BooksModel> getBooksAsync()
        {

            try
            {
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-type", "application/json;charset=utf-8");

                #pragma warning disable CS0618 // Type or member is obsolete
                var response = await _restClientGetAll.ExecuteTaskAsync<BooksModel>(request);
                #pragma warning restore CS0618 // Type or member is obsolete


                if (response.Content != null)
                    return JsonConvert.DeserializeObject<BooksModel>(response.Content);
                else
                    return null;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// API call to get the book detail
        /// </summary>
        public static async Task<Item> getBookDetailAsync(string volumeId)
        {

            try
            {
                var request = new RestRequest(string.Format(bookDetail, volumeId), Method.GET);
                request.AddHeader("Content-type", "application/json;charset=utf-8");

                #pragma warning disable CS0618 // Type or member is obsolete
                var response = await _rest.ExecuteTaskAsync<Item>(request);
                #pragma warning restore CS0618 // Type or member is obsolete


                if (response.Content != null)
                    return JsonConvert.DeserializeObject<Item>(response.Content);
                else
                    return null;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

}
