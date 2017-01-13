using EFolderDomain.Model;
using System;
using System.Collections.Generic;

namespace EFolderDomain.Infrastructure.DTO
{
    public class BackupDTO
    {
        public string date { get; private set; }
        public int backupId { get; private set; }
        public string status { get; private set; }

        public static IEnumerable<BackupDTO> Map(IEnumerable<Backup> backups)
        {
            List<BackupDTO> result = new List<BackupDTO>();

            foreach(Backup bk in backups)
            {
                BackupDTO dto = new BackupDTO();
                dto.date = bk.Date.ToString(@"yyyy-MM-dd HH:mm:ss");
                dto.backupId = bk.Id;
                dto.status = Enum.GetName(typeof(BackupStatus), bk.Status);
                result.Add(dto);
            }

            return result;
        }
    }
}
