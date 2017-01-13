using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    // It is iterator through ToDo items stream
    public interface IToDoItemStreamParser
    {
        // Init iterator with ToDo items stream
        void Init(Stream input);
        bool Next();
        ToDoItem Current();
    }
}
