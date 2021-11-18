using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Static
{
    public static class Endpoints
    {
#if DEBUG
        public static string BaseUrl = "https://localhost:44362/";
#else
        public static string BaseUrl = "https://bookstore-api20211118163601.azurewebsites.net/";
#endif
        public static string AuthorsEndpoint = $"{BaseUrl}api/authors/";
        public static string BooksEndpoint = $"{BaseUrl}api/books/";
        public static string RegisterEndpoint = $"{BaseUrl}api/users/register/";
        public static string LoginEndpoint = $"{BaseUrl}api/users/login/";
    }
}