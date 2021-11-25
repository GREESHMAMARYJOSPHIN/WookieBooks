using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Xml;

namespace WookieBooks.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));

        public LoggerManager()
        {
            XmlDocument log4netconfig = new XmlDocument();
            using (var filestream = File.OpenRead("log4net.config"))
            {
                //Initializing logger
                log4netconfig.Load(filestream);
                var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                XmlConfigurator.Configure(repo, log4netconfig["log4net"]);

                _logger.Info("Log initialized");
            }
        }

        public void LogInfo(string Message)
        {
            //log Message at Info level
            _logger.Info(Message);
        }
    }
}
