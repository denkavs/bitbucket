using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Infrastructure
{
    public class internalToDoItem
    {
        public string subject { get; set; }
        public string dueDate { get; set; } // format: yyyy-MM-dd HH:mm:ss
        public bool done { get; set; }
    }

    public class internalUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public List<internalToDoItem> todos { get; set; }
    }

    // Functionality of this class is used to generate test data for Third part JAVA service
    public class UsersServiceGenerator
    {
        private static int userIndex = 0;
        public static async Task<bool> Generate(string serviceUri, string servicePath)
        {
            bool res = false;
            if(!string.IsNullOrEmpty(serviceUri) && !string.IsNullOrEmpty(servicePath))
            {
                string baseUri = serviceUri;
                string query = servicePath;

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<internalUser> users = GenerateUsers();

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync<IEnumerable<internalUser>>(query, users);
                    Logger.Log.Info(string.Format("New porsion of test users for JAVA service generated. User count - {0}", users.Count));
                }
                catch(Exception e)
                {
                    Logger.Log.Error(e.Message, e);
                }
                res = true;
            }

            return res;
        }

        private static List<internalUser> GenerateUsers()
        {
            List<internalUser> users = new List<internalUser>();
            string suffix = string.Empty;
            string[] mails = { "@gmail.com", "@yahoo.com", "@msn.com", "@amazon.com", "@ibm.io"};

            Random rnd = new Random();
            int count = rnd.Next(1, 5);
            while(count > 0)
            {
                suffix = DateTime.UtcNow.Ticks.ToString().Substring(14, 4);
                count--;
                internalUser user = new internalUser();
                string name = "User_" + suffix;
                user.id = ++userIndex;
                user.username = name;
                user.email = name + mails[rnd.Next(0, 5)];
                addToDoItems(user, rnd.Next(0, 6));
                users.Add(user);
            }

            return users;
        }

        private static void addToDoItems(internalUser user, int count)
        {
            user.todos = new List<internalToDoItem>();
            bool done = true;

            for (int i = 0; i < count; i++)
            {
                internalToDoItem todo = new internalToDoItem();
                todo.subject = "subject_" + user.username + "_N_" + i.ToString();
                todo.dueDate = DateTime.UtcNow.AddDays(i).ToString("yyyy-MM-dd HH:mm:ss");
                done = !done;
                todo.done = done;
                user.todos.Add(todo);
            }
        }
    }
}
