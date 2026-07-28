using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Helpers
{
    public static class JsonExtensions
    {
        public static async Task<T?> ReadAsync<T>(this HttpResponseMessage response)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}