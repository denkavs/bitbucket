using ConsoleBackUp.Infrastructure;
using EFolderDomain.Infrastructure;
using EFolderDomain.Infrastructure.DTO;
using EFolderDomain.Interfaces;
using EFolderDomain.Model;
using EFolderDomain.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleBackUp.Controllers
{
    public class BackupsController : ApiController
    {
        private IBackupService service;

        public BackupsController(IBackupService service)
        {
            this.service = service;
        }

        public IEnumerable<BackupDTO> GetBackups()
        {
            return BackupDTO.Map(this.service.ListBackup());
        }

        [BackUpThrottleFilter]
        public async Task<PlainBackUpDTO> PostBackups()
        {
            // This code generates new portion of test users with todo items for JAVA service.
            IConfigurationMgr conf = new ConfMgr();
            bool res = UsersServiceGenerator.Generate(conf.GetToDoItemServiceUri(), conf.GetToDoItemServicePath()).Result;

            Backup backup = await this.service.MakeBackup();
            PlainBackUpDTO dto = new PlainBackUpDTO();
            dto.backupId = backup.Id;
            return dto;
        }
    }
}
