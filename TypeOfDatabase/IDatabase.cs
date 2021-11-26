using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    public interface IDatabase
    {
        string ConnectionString { get; set; }

        bool Connect(string ip, string username, string pass, string dbName = "");

        bool Disconnect();

        List<string> ReadAllTheFields();

        List<Dictionary<string, string>> ReadEverything();
        void CreateClasses(List<string> list);

        void SetScript();
    }
}
