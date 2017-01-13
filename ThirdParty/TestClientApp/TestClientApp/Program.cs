using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TestClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit from program...");
            Console.ReadKey();

            RunClients();
        }

        static void RunClients()
        {
            string baseUri = (string)ConfigurationManager.AppSettings["ServiceBaseUri"];
            string query = (string)ConfigurationManager.AppSettings["ServiceQueryPath"];
            int timeBetweenRequests = Convert.ToInt32(ConfigurationManager.AppSettings["TimeBetweenRequestsInMilliseconds"]);
            int countOfRequest = Convert.ToInt32(ConfigurationManager.AppSettings["RequestCount"]);

            Console.WriteLine("Application is run with next parameters to emulate requests:");
            Console.WriteLine("Request uri - [{0}]", baseUri + query);
            Console.WriteLine("Time between requests - [{0}]", timeBetweenRequests);
            Console.WriteLine("Count of requests - [{0}]", countOfRequest);

            for (int i = 0; i <= countOfRequest; i++)
            {
                int index = i + 1;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Console.WriteLine("Request [{0}] started.", index);
                        HttpClient client = new HttpClient();

                        client.BaseAddress = new Uri(baseUri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.PostAsync(query, null).Result;
                        Console.WriteLine(string.Format("Request [{0}] successfully completed. Status code [{1}]", index, response.StatusCode));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Request [{0}] failed.", index);
                    }
                });

                if(timeBetweenRequests != 0)
                    Thread.Sleep(timeBetweenRequests);
            }
        }
    }
}
