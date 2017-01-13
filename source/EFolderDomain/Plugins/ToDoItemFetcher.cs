using EFolderDomain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using EFolderDomain.Infrastructure;

namespace EFolderDomain.Plugins
{
    public class ToDoItemFetcher : IToDoItemFetcher
    {
        private string baseUri;
        private string query;

        public ToDoItemFetcher(string baseUri, string query)
        {
            this.baseUri = baseUri;
            this.query = query;
        }

        public async Task<Stream> Fetch()
        {
            Stream res = null;

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(this.baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = null;

            try
            {
                response = await client.GetAsync(this.query);
            }
            catch (Exception e)
            {
                Logger.Log.Error("JAVA service is not reachable....", e);
                throw;
            }
            
            if (response != null && response.IsSuccessStatusCode)
            {
                try
                {
                    res = await response.Content.ReadAsStreamAsync();
                }
                catch (Exception e)
                {
                }
            }
            return res;
        }
    }
}
