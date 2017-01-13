using EFolderDomain.Interfaces;
using EFolderDomain.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Plugins
{
    public class ToDoItemStreamParser: IToDoItemStreamParser, IDisposable
    {
        private JsonReader reader = null;
        private Stream sr = null;
        private TextReader tr = null;
        private ToDoItem current = null;
        private string userName;

        public void Init(Stream stream)
        {
            if (stream != null)
            {
                this.sr = stream;
                this.tr = new StreamReader(this.sr);
                this.reader = new JsonTextReader(this.tr);
            }
        }

        public ToDoItem Current()
        {
            return this.current;
        }

        public bool Next()
        {
            if (this.reader != null)
            {
                this.current = null;
                this.current = this.ReadNextToDoItem();

                if (this.current == null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        private ToDoItem ReadNextToDoItem()
        {
            while (this.reader.Read())
            {
                JsonToken tocken = this.reader.TokenType;
                var value = this.reader.Value;
                if (tocken == JsonToken.PropertyName && (string)this.reader.Value == "username")
                {
                    // get username value
                    this.reader.Read();
                    this.userName = (string)reader.Value;
                }
                if (shouldInitToDoItem(this.reader))
                {
                    this.current = getToDoItem(this.reader, this.userName);
                    return this.current;
                }
            }

            return null;
        }

        private ToDoItem getToDoItem(JsonReader reader, string userName)
        {
            ToDoItem todo = new ToDoItem();
            todo.UserName = userName;

            while (reader.TokenType != JsonToken.EndObject && reader.Path.Contains("todos"))
            {
                reader.Read();
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propName = (string)reader.Value;
                    // get value for corresponding property
                    reader.Read();
                    var propValue = reader.ValueType;
                    MatToDoItemProp(todo, propName, reader.Value);
                }
            }

            return todo;
        }

        private void MatToDoItemProp(ToDoItem todo, string propName, Object propValue)
        {
            switch (propName)
            {
                case "id":
                    {
                        todo.ToDoItemId = Convert.ToInt32(propValue);
                        break;
                    }
                case "subject":
                    {
                        todo.Subject = Convert.ToString(propValue);
                        break;
                    }
                case "dueDate":
                    {
                        todo.DueDate = DateTime.Parse(Convert.ToString(propValue));
                        break;
                    }
                case "done":
                    {
                        todo.Status = Convert.ToBoolean(propValue) ? ToDoItemStatus.Done : ToDoItemStatus.None;
                        break;
                    }
                default: break;
            }
        }

        private bool shouldInitToDoItem(JsonReader reader)
        {
            if (reader != null)
            {
                if (reader.TokenType == JsonToken.StartObject && reader.Path.Contains("todos"))
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (this.reader != null)
            {
                this.reader.Close();
                this.reader = null;
            }

            if (this.tr != null)
            {
                this.tr.Close();
                this.tr = null;
            }

            if (this.sr != null)
            {
                this.sr.Close();
                this.sr = null;
            }
        }
    }
}
