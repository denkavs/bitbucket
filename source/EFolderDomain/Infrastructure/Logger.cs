using log4net;
using System;
using System.IO;

namespace EFolderDomain.Infrastructure
{
    public class Logger
    {
        private static readonly ILog logForNet = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Logger() { }
        private Logger()
        {
        }

        public static ILog Log
        {
            get { return logForNet; }
        }

        public static void InitLogger(string l4nfile)
        {
            if (!string.IsNullOrEmpty(l4nfile))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(l4nfile));
                logForNet.Info("log4net logger is initialazed.");
            }
        }
    }
}
