using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBackUp.Infrastructure
{
    public class ConfMgr: IConfigurationMgr
    {
        public string GetAppServiceUri()
        {
            return (string)ConfigurationManager.AppSettings[Consts.AppServiceUriConfigName];
        }

        public string GetToDoItemServiceUri()
        {
            return (string)ConfigurationManager.AppSettings[Consts.SrcToDoItemServiceUri];
        }

        public string GetToDoItemServicePath()
        {
            return (string)ConfigurationManager.AppSettings[Consts.SrcToDoItemServicePath];
        }
        public static int GetMaxNumberOfBackUpConcarrentOperations()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[Consts.MaxNumberOfBackUpConcarrentOperations]);
        }
    }
}
