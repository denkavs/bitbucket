using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    // It is used to iterate through ToDo items in stream returned by JAVA service
    public interface IToDoItemStreamParser
    {
        // Init iterator with ToDo items stream
        void Init(Stream input);
        bool Next();
        ToDoItem Current();
    }
}
