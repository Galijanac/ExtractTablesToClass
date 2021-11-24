using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            IDatabase database;
            Dictionary<string, string> configuration = new Dictionary<string, string>()
            {
                {"ip", "localhost" },
                {"user", "sa"},
                {"password", string.Empty },
                {"dbname", "master"},
                {"tablename", string.Empty },
                {"namespace", string.Empty },
                {"filename", "class.cs" }
            };

            SetConfiuration(args, ref configuration);

            Console.WriteLine("Choose your database :");
            Console.WriteLine("For SQL Server type 1");

            int type;

            if(!int.TryParse(Console.ReadLine(),out type))
            {
                return;
            }

            switch (type)
            {
                case 1:
                    database = new SQLServer();
                    break;
                default:
                    return;
            }

            if (!database.connect(configuration["ip"], configuration["user"], configuration["password"], configuration["dbname"]))
            {
                Console.WriteLine("Bad configuration");
                return;
            }

            List<string> list = database.readAllTheFIelds(configuration["tablename"]);
            database.disconnect();       

            File.WriteAllText(configuration["filename"], GenerateCode(configuration["tablename"], configuration["namespace"], list));
        }

        private static string GenerateCode(string tablename, string nameSpace, List<string> list)
        {
            string code = "using System.ComponentModel;";
            code += $"\n namespace {nameSpace}\n{{";
            code += $"\nclass {tablename} : INotifyPropertyChanged  \n {{";
            code += "\npublic event PropertyChangedEventHandler PropertyChanged;";

            foreach (string columnName in list)
            {
                code += $"\n private string {columnName.ToLower()};";
            }

            code += "\n \n";

            foreach (string columnName in list)
            {
                code += $"\n public string {columnName.ToUpper()}";
                code += "\n {";
                code += "\n     get";
                code += "\n     {";
                code += $"\n     return {columnName.ToLower()};";
                code += "\n     }";
                code += "\n     set";
                code += "\n     {";
                code += $"\n        {columnName.ToLower()} = value;";
                code += $"\n        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(\"{columnName.ToLower()}\"));";
                code += "\n     }";
                code += "\n}";

            }

            code += "\n}\n}";
            return code;
        }

        private static void SetConfiuration(string[] args, ref Dictionary<string, string> configuration)
        {
            for(int i = 0; i < args.Length; i = i + 2)
            {
                if (configuration.Keys.Contains(args[i]))
                {
                    configuration[args[i]] = args[i + 1];
                }
            }

            if (configuration["password"] == string.Empty)
            {
                Console.WriteLine("Please insert password:");
                configuration["password"] = Console.ReadLine();
            }

            if (configuration["tablename"] == string.Empty)
            {
                Console.WriteLine("Please insert table name:");
                configuration["tablename"] = Console.ReadLine();
            }

            if (configuration["namespace"] == string.Empty)
            {
                Console.WriteLine("Please insert namespace:");
                configuration["namespace"] = Console.ReadLine();
            }

            DisplayConfiguration(configuration);
        }

        private static void DisplayConfiguration(Dictionary<string, string> configuration)
        {
            foreach(var pair in configuration)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
            Console.ReadLine();
        }
    }
}
