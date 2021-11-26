using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractTablesToClasses.Helper
{
    public class CreateFillScript
    {
        public static string FillAS400(Dictionary<string,string> informations)
        {
            string querry = $"INSERT INTO {Program.DbName}.{Program.TableName.ToUpper()}";

            string columns = "(";
            string values = "(";

            foreach(KeyValuePair<string,string> pair in informations)
            {
                columns += $" {pair.Key.ToUpper()},";
                values += $" '{pair.Value}',";
            }

            return $"{querry}{columns.Remove(columns.Length-1)}) VALUES {values.Remove(values.Length-1)});";
        }
    }
}
