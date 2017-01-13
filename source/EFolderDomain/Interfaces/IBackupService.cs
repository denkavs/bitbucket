using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    /*
     * Interface represent application specific business rules. It encapsulates and implements all of the use cases of the system.
     */
    public interface IBackupService
    {
        Task<Backup> MakeBackup();
        IEnumerable<Backup> ListBackup();
        Task<IEnumerator<ToDoItem>> ExportToDoItems(int backUpId);
    }
}
