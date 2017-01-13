using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Model
{
    public enum BackupStatus
    {
        None,
        InProgress,
        Ok,
        Failed
    }
    public class Backup
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public BackupStatus Status { get; set; }
    }
}
