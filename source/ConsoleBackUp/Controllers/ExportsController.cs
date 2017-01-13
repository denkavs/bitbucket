using EFolderDomain.Interfaces;
using EFolderDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleBackUp.Controllers
{
    public class ExportsController : ApiController
    {
        private IBackupService service;

        public ExportsController(IBackupService service)
        {
            this.service = service;
        }

        public Task<IEnumerator<ToDoItem>> GetToDoItems(int Id)
        {
            return this.service.ExportToDoItems(Id);
        }
    }
}
