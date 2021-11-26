using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnu.Getopt;
using LogWrapperNamespace;

namespace ExtractTablesToClasses
{
    class Program
    {
        public static string IP { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string DbName { get; set; }
        public static string  TableName { get; set; }
        public static string  Namespace { get; set; }
        public static string  FileName { get; set; }
        public static string DbType { get; set; }
        public static string Option { get; set; }
        public static LogWrapper LogWrapper { get; set; }
        static void Main(string[] args)
        {
            SetParameters(args);
            IDatabase database;

            switch (DbType)
            {
                case "1":
                    database = new SQLServer();
                    break;
                case "2":
                    database = new DB2();
                    break;
                default:
                    return;

            }

            if (!database.Connect(IP, User, Password, DbName))
            {
                LogWrapper.NotifyLogError("Bad configuration");
                return;
            }

            switch (Option.ToLower())
            {
                case "class":
                    List<string> list = database.ReadAllTheFields();
                    database.CreateClasses(list);
                    break;
                case "script":
                    database.SetScript();
                    break;
                default:
                    return;
            }

            database.Disconnect();

            Console.ReadLine();
        }


        public static void SetParameters(string[] args)
        {
            LongOpt[] longopts =
            {
                new LongOpt("ip", Argument.Optional, null, '1'),
                new LongOpt("user", Argument.Optional, null, '2'),
                new LongOpt("password", Argument.Optional, null, '3'),
                new LongOpt("dbname", Argument.Optional, null, '4'),
                new LongOpt("dbtype", Argument.Optional, null, '5'),
                new LongOpt("tablename", Argument.Optional, null, '6'),
                new LongOpt("namespace",Argument.Optional,null,'7'),
                new LongOpt("filename",Argument.Optional,null,'8'),
                new LongOpt("logname",Argument.Optional,null,'9'),
                new LongOpt("option",Argument.Optional,null,10)
            };

            Getopt g = new Getopt("AmazonKinesisTest", args, "i:u:p:d:b:t:n:f:l:o:", longopts);
            int c;

            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'i':
                    case '1':
                        IsEmpty(g.Optarg, "IP value is empty");
                        IP = g.Optarg;
                        break;
                    case 'u':
                    case '2':
                        IsEmpty(g.Optarg, "User value is empty");
                        User = g.Optarg;
                        break;
                    case 'p':
                    case '3':
                        IsEmpty(g.Optarg, "Password value is empty");
                        Password = g.Optarg;
                        break;
                    case 'd':
                    case '4':
                        IsEmpty(g.Optarg, "DbName value is empty");
                        DbName = g.Optarg;
                        break;
                    case 'b':
                    case '5':
                        IsEmpty(g.Optarg, "DbType value is empty");
                        DbType = g.Optarg;
                        break;
                    case 't':
                    case '6':
                        IsEmpty(g.Optarg, "TableName value is empty");
                        TableName = g.Optarg;
                        break;
                    case 'n':
                    case '7':
                        IsEmpty(g.Optarg, "Namespace value is empty");
                        Namespace = g.Optarg;
                        break;
                    case 'f':
                    case '8':
                        IsEmpty(g.Optarg, "File name is empty");
                        FileName = g.Optarg;
                        break;
                    case 'l':
                    case '9':
                        IsEmpty(g.Optarg, "Log file value is empty");
                        SetLogParameters(g.Optarg);
                        break;
                    case 'o':
                    case 10:
                        IsEmpty(g.Optarg, "File name is empty");
                        Option = g.Optarg;
                        break;
                }
            }
        }
        public static void SetLogParameters(string logName)
        {
            LogWrapper = new LogWrapper();
            LogWrapper.IsConsole = Environment.UserInteractive;
            LogWrapper.PathLog = AppDomain.CurrentDomain.BaseDirectory;
            LogWrapper.LogName = logName;
            LogWrapper.NumberOfFile = 10;
            LogWrapper.HasFileDate = false;
            LogWrapper.Color = ConsoleColor.DarkYellow;
            LogWrapper.HasLogDate = false;
            LogWrapper.ClearLog();
        }

        public static void IsEmpty(string s, string error)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new Exception(error);
            }
        }
    }
}
