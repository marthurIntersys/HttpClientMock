﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExampleWebApi.Service
{
    public interface IHttpClientDependentService
    {
        IEnumerable<string> GetValues();
        string GetValue(int id);
    }

    public class HttpClientDependentService : IHttpClientDependentService
    {
        private const string LOCAL_BASE_URL = "https://localhost:44390";
        private HttpClient _httpClient;

        public HttpClientDependentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<string> GetValues()
        {
            string rawValues;
            string[] returnValue;

            try
            {
                rawValues = _httpClient.GetAsync($"{LOCAL_BASE_URL}/api/values").Result.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw new Exception("Caught exception while calling api/values.");
            }


            try
            {
                returnValue = JsonConvert.DeserializeObject<string[]>(rawValues);
            }
            catch
            {
                throw new Exception("Caught exception while parsing result from api/values.");
            }

            return returnValue;
        }

        public string GetValue(int id)
        {
            string rawValue;

            try
            {
                rawValue = _httpClient.GetAsync($"{LOCAL_BASE_URL}/api/values/{id}").Result.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw new Exception($"Caught exception while calling api/values/{id}.");
            }

            return rawValue;
        }
    }
}
