using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    public interface IDatabase
    {
        string connectionString { get; set; }

        List<string> readAllTheFIelds(string tableName);

        bool connect(string ip, string username, string pass, string dbName = "");

        bool disconnect();
    }
}
