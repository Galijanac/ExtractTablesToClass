using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses
{
    class SQLServer : IDatabase
    {
        private string dbName;
        private SqlConnection cnn;

        public string connectionString { get; set; }

        public bool connect(string ip, string username, string pass, string dbName = "")
        {
            this.dbName = dbName;
            connectionString = $@"Data Source={ip};Initial Catalog={dbName};User ID={username};Password={pass}";

            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool disconnect()
        {
            try
            {
                cnn.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public List<string> readAllTheFIelds(string tableName)
        {
            List<string> list = new List<string>();
            string querry = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

            string temp;
            using (SqlCommand command = new SqlCommand(querry, cnn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        temp = string.Empty;

                        list.Add(reader.GetValue(0).ToString());
                    }
                }
            }

            return list;
        }
    }
}
