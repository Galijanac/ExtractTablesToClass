using ExtractTablesToClasses.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    class SQLServer : IDatabase
    {
        private string dbName;
        private SqlConnection cnn;

        public string ConnectionString { get; set; }

        public bool Connect(string ip, string username, string pass, string dbName = "")
        {
            this.dbName = dbName;
            ConnectionString = $@"Data Source={ip};Initial Catalog={dbName};User ID={username};Password={pass}";

            cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();
                Program.LogWrapper.NotifyLog("Database connected");
            }
            catch
            {
                Program.LogWrapper.NotifyLogError("Database connection error");
                return false;
            }

            return true;
        }

        public bool Disconnect()
        {
            try
            {
                cnn.Close();
                Program.LogWrapper.NotifyLog("Database closed");
            }
            catch
            {
                Program.LogWrapper.NotifyLogError("Database closing error");
                return false;
            }

            return true;
        }

        public List<string> ReadAllTheFields()
        {
            List<string> list = new List<string>();
            string querry = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{Program.TableName}'";

            using (SqlCommand command = new SqlCommand(querry, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue(0).ToString().Trim());
                    }
                }
            }
            Program.LogWrapper.NotifyLog("Information form database extracted");
            return list;
        }

        public List<Dictionary<string,string>> ReadEverything()
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            string querry = $"SELECT * FROM {Program.TableName}";
            List<string> fields = ReadAllTheFields();

            using (SqlCommand command = new SqlCommand(querry, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();

                        for (int i = 0; i < fields.Count; i++)
                        {
                            dictionary.Add(fields[i], reader.GetValue(i).ToString().Trim());
                        }

                        list.Add(dictionary);
                    }
                }
            }

            return list;
        }

        public void CreateClasses(List<string> list)
        {
            File.WriteAllText(Program.FileName, GenerateCode(Program.TableName, Program.Namespace, list));
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

        public void SetScript()
        {
            string script = string.Empty;
            List<Dictionary<string, string>> everything = ReadEverything();
            foreach (Dictionary<string, string> dictionary in everything)
            {
                script += $"{CreateFillScript.FillAS400(dictionary)}\n\n";
            }

            File.WriteAllText(Program.FileName, script);
        }
    }
}
