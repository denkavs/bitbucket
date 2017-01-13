using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Interfaces
{
    /*
     * Provides functionality to get configuration data.
     */
    public interface IConfigurationMgr
    {
        // Returns base uri of application service
        string GetAppServiceUri();
        // Returns base uri for Third part JAVA service
        string GetToDoItemServiceUri();

        // Returns Third part JAVA service path that is used to get ToDo items.
        string GetToDoItemServicePath();
    }
}
