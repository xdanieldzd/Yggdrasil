using System;

namespace Yggdrasil
{
    static partial class BuildInformation
    {
        static BuildInformation()
        {
            data.Add("BuildDate", new DateTime(0));
            data.Add("GitBranch", string.Empty);
            data.Add("LatestCommitHash", string.Empty);
            data.Add("BuildMachineName", string.Empty);
            data.Add("BuildMachineOSPlatform", string.Empty);
            data.Add("BuildMachineOSVersion", string.Empty);
            data.Add("BuildMachineProcessorArchitecture", string.Empty);
        }
    }
}
