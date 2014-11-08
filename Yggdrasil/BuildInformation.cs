using System;
using System.Collections.Generic;

namespace Yggdrasil
{
    static partial class BuildInformation
    {
        static Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

        /* https://stackoverflow.com/a/3510547 */
        public sealed class BuildInformationIndexer
        {
            public dynamic this[string name]
            {
                get { return (BuildInformation.data.ContainsKey(name) ? BuildInformation.data[name] : "unset"); }
            }

            public Dictionary<string, dynamic> GetProperties()
            {
                return data;
            }
        }

        static BuildInformationIndexer indexer;
        public static BuildInformationIndexer Properties
        {
            get { return indexer ?? (indexer = new BuildInformationIndexer()); }
        }
    }
}
