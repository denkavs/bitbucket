using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFolderDomain.Model;

namespace EFolderDomain.Plugins
{
    public class BackupRepository : IBackupRepository
    {
        private readonly IDictionary<int, Backup> repo;
        private readonly object _mutex = new object();

        public BackupRepository()
        {
            this.repo = new Dictionary<int, Backup>();
        }

        public Backup Add(Backup bk)
        {
            lock (this._mutex)
            {
                int id = this.repo.Count + 1;
                bk.Id = id;
                this.repo.Add(id, bk);
            }
            return bk;
        }

        public bool Delete(int backupId)
        {
            lock (this._mutex)
            {
                if (this.repo.ContainsKey(backupId))
                {
                    return this.repo.Remove(backupId);
                }
                else
                    return false;
            }
        }

        public bool Edit(Backup bk)
        {
            lock (this._mutex)
            {
                int key = bk.Id;
                if (this.repo.ContainsKey(key))
                {
                    this.repo[key].Status = bk.Status;
                    this.repo[key].Date = bk.Date;
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<Backup> List()
        {
            return this.repo.Select(i => i.Value).ToList();
        }
    }
}
