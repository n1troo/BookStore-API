using Blazored.LocalStorage;
using BookStore_UI_ServerSide.Contracts;
using BookStore_UI_ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Service
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory _Client;
        private readonly ILocalStorageService _localStorageService;

        public AuthorRepository(IHttpClientFactory client, ILocalStorageService localStorageService) :base(client, localStorageService)
        { 
            _localStorageService = localStorageService;
            _Client = client;
        }
    }
}
