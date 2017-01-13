using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    /*
     * This interface represents methods to work with repository that keeps backups.
     */
    public interface IBackupRepository
    {
        Backup Add(Backup bk);
        bool Edit(Backup bk);
        bool Delete(int backupId);
        IEnumerable<Backup> List();
    }
}
