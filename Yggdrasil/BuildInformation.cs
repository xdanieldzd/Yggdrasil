using System;

namespace Yggdrasil
{
	static class BuildInformation
    {
        public static readonly DateTime BuildDate = new DateTime(0);
		public static readonly string GitBranch = string.Empty;
        public static readonly string LatestCommitHash = string.Empty;
		public static readonly bool HasPendingChanges = false;
		public static readonly string BuildMachineName = string.Empty;
		public static readonly string BuildMachineOSPlatform = string.Empty;
		public static readonly string BuildMachineOSVersion = string.Empty;
		public static readonly string BuildMachineProcessorArchitecture = string.Empty;
    }
}
