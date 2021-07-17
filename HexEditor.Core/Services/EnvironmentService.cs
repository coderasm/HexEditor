using System;

namespace HexEditor.Core.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public string MachineName => Environment.MachineName;

        public string OperatingSystem => Environment.OSVersion.ToString();

        public string UserDomainName => Environment.UserDomainName;

        public string UserName => Environment.UserName;

        public string CLRVersion => Environment.Version.ToString();
    }

    public interface IEnvironmentService
    {
        string MachineName { get; }
        string OperatingSystem { get; }
        string UserDomainName { get; }
        string UserName { get; }
        string CLRVersion { get; }
    }
}
