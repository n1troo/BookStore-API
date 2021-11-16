using Blazored.LocalStorage;
using BookStore_UI_ServerSide.Contracts;
using BookStore_UI_ServerSide.Models;
using BookStore_UI_ServerSide.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Service
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public BaseRepository(IHttpClientFactory httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task<bool> Create(string url, T obj)
        {
            if (obj == null) { return false; }


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            };


            ////ignorujemy serializacje innych dalszych powiazan
            //var objcontent = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url)
            //{
            //    //request.Content = JsonContent.Create(new { user });
            //    Content = new StringContent(objcontent, Encoding.UTF8, "application/json")
            //};

            var client = _httpClient.CreateClient();
            //dolaczenie tokena autoryazacji do kazdego zapytania
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearereToken());
            
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created) 
            {
                return true; 
            }

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1) { return false; }

            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _httpClient.CreateClient();
            //dolaczenie tokena autoryazacji do kazdego zapytania
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearereToken());
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) { return true; }

            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Endpoint url"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get(string url, int id)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _httpClient.CreateClient();
            //dolaczenie tokena autoryazacji do kazdego zapytania
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearereToken());
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var back = JsonConvert.DeserializeObject<T>(content);
                return back;
            }

            return null;

        }

        public async Task<IList<T>> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClient.CreateClient();
            //dolaczenie tokena autoryazacji do kazdego zapytania
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearereToken());
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var back = JsonConvert.DeserializeObject<IList<T>>(content);
                return back;
            }

            return null;
        }

        public async Task<bool> Update(string url, T obj, int Id)
        {

            if (obj == null)
                return false;
            try
            {
                //ignorujemy serializacje innych dalszych powiazan
                var objcontent = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }) ;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url + Id)
                {
                    //request.Content = JsonContent.Create(new { user });
                    Content = new StringContent(objcontent, Encoding.UTF8, "application/json")
                };

                var client = _httpClient.CreateClient();
                //dolaczenie tokena autoryazacji do kazdego zapytania
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetBearereToken());
                var response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            { 
                return false;
            }
        }

        private async Task<string> GetBearereToken()
        {
            return await _localStorageService.GetItemAsync<string>("authToken");
        }
    }
}
