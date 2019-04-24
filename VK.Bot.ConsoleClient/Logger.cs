using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace VK.Bot.ConsoleClient
{
    internal static class Logger
    {
        public static void Init()
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                typeof(Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}