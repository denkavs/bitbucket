using EFolderDomain.Interfaces;
using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Infrastructure
{
    public class CsvToDoFormatter : MediaTypeFormatter
    {
        private string mimeFormat = "text/csv"; // "text/tab-separated-values" mime type is used for .tsv file.
        public CsvToDoFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(this.mimeFormat));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }
        public override bool CanWriteType(Type type)
        {
            return type == typeof(IEnumerator<ToDoItem>);
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            string todoStrings = string.Empty;

            IEnumerator<ToDoItem> enumerator = (IEnumerator<ToDoItem>)value;
            StreamWriter writer = new StreamWriter(writeStream);

            // add header of table
            StringBuilder sb = new StringBuilder();
            sb.Append("Username;TodoItemId;Subject;DueDate;Done");
            sb.AppendLine();
            await writer.WriteAsync(sb.ToString());

            if(enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    ToDoItem todo = enumerator.Current;
                    todoStrings = getStringForStream(new List<ToDoItem>() { todo });
                    await writer.WriteAsync(todoStrings);
                }
            }

            writer.Flush();
        }

        protected string getStringForStream(IEnumerable<ToDoItem> items)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ToDoItem todo in items)
            {
                sb.Append(string.Format(@"{0};{1};{2};{3};{4}", prepareString(todo.UserName), todo.ToDoItemId, prepareString(todo.Subject), todo.DueDate.ToString("MM/dd/yy H:mm"), todo.Status == ToDoItemStatus.Done ? "Done" : ""));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string prepareString(string input)
        {
            // , "
            if (input.Contains(',') || input.Contains('"'))
            {
                return string.Format(@"""{0}""", input.Replace("\"", "\"\""));
            }

            return input;
        }
    }
}
