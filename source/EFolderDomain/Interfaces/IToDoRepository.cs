using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    // Represents todo item repository
    public interface IToDoRepository
    {
        ToDoItem Add(ToDoItem item, int backUpId);
        IEnumerator<ToDoItem> GetEnumerator(int backUpId);
    }
}
