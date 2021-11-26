using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    class DB2 : IDatabase
    {
        public string ConnectionString { get; set; }

        public bool Connect(string ip, string username, string pass, string dbName = "")
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public List<string> ReadAllTheFields()
        {
            throw new NotImplementedException();
        }

        public void CreateClasses(List<string> list)
        {

        }

        public List<Dictionary<string, string>> ReadEverything()
        {
            throw new NotImplementedException();
        }

        public void SetScript()
        {
            throw new NotImplementedException();
        }
    }
}
