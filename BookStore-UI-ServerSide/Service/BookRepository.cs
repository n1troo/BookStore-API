using Blazored.LocalStorage;

using BookStore_UI_ServerSide.Contracts;
using BookStore_UI_ServerSide.Models;

using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Service
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;

        public BookRepository(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
            : base (httpClientFactory, localStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
        }
    }
}
