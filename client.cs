using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    class Class1
    {
        static async Task<string> GetURI(Uri u)
        {
            var response = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        public string GetData()
        {
            var t = Task.Run(() => GetURI(new Uri("https://localhost:44368/api/Musics")));
            t.Wait();
            return t.ToString();
        }

    }

}
