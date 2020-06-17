using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Mabeعaty
{
    public class Sql_interface
    {

        private String dbConnection;
        /// <summary>
        /// to create new database file 
        /// input file must be equal to data source
        /// </summary>
        /// <param name="connectionOpts">connection options </param>
        public Sql_interface(Dictionary<String, String> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            dbConnection = str;



        }
        /// <summary>
        /// private function to executeNonQuery function
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        private int ExecuteNonQuery(string sql)
        {
            int rowsUpdated = 0;

            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    rowsUpdated = mycommand.ExecuteNonQuery();
                }
                cnn.Close();
            }
            return rowsUpdated;
        }
        /// <summary>
        /// to create new table
        /// </summary>
        /// <param name="tableName">table name to be created</param>
        /// <param name="fields">parameters of the table and their types </param>
        /// <returns></returns>
        public bool create_newTable(string tableName, Dictionary<String, String> fields)
        {
            Boolean returnCode = true;
            String Table_fields = "";
            foreach (KeyValuePair<String, String> row in fields)
            {
                Table_fields += String.Format("{0} {1}, ", row.Key, row.Value);
            }
            Table_fields = Table_fields.Trim().Substring(0, Table_fields.Length - 1);
            Table_fields = Table_fields.Remove(Table_fields.Length - 1, 1);

            try
            {
                this.ExecuteNonQuery(String.Format("CREATE TABLE IF NOT EXISTS {0}({1})", tableName, Table_fields));
            }
            catch (Exception ex)
            {
                returnCode = false;
            }
            return returnCode;

        }
        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="tableName">The table to insert into</param>
        /// <param name="columns">A String containing Column names in form "column_1, column_2"</param>
        /// <param name="values">A String containing values in corresponding Column names in form "'value_1', 'value_2'"</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Insert(String tableName, String columns, String values)
        {

            Boolean returnCode = true;

            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception ex)
            {
                returnCode = false;
            }
            return returnCode;
        }
        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="vals">A String containing Column names and their new values in form "column_1 = 'value_1', column_2 = 'value_2' ".</param>
        /// <param name="where">The where clause for the update statement in form "column_name = 'value'".</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(String tableName, String vals, String where)
        {
            Boolean returnCode = true;

            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }
        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete in form "column_name = 'value'".</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch (Exception ex)
            {
                returnCode = false;
            }
            return returnCode;
        }
        /// <summary>
        /// select a table 
        /// </summary>
        /// <param name="numof_columns">number of colums to be brought</param>
        /// <param name="Table_name"></param>
        public List<string> select_all(int numof_columns, string Table_name)
        {
            List<string> ret = new List<string>();
            string sql = "Select * from " + Table_name;
            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
                {
                    cnn.Open();
                    using (SQLiteCommand mycommand = new SQLiteCommand(cnn))
                    {
                        mycommand.CommandText = sql;
                        using (SQLiteDataReader reader = mycommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    for (int i = 0; i < numof_columns; i++)
                                        ret.Add(reader.GetValue(i).ToString());

                                }
                            }
                            reader.Close();
                        }
                    }
                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("error occured");
            }
            return ret;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Table_name">The table from which to select </param>
        /// <param name="columns">A String containing Column names to be fetched in form "column_1, column_2" </param>
        /// <param name="numof_columns">number of colums to be fetched</param>
        /// <param name="where">The where clause for the select in form "column_name = 'value'"</param>
        /// <returns></returns>
        public List<string> Select_item(string Table_name, string columns, int numof_columns, string where)
        {

            List<string> ret = new List<string>();
            try
            {
                using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
                {
                    cnn.Open();
                    using (SQLiteCommand mycommand = new SQLiteCommand(cnn))
                    {
                        mycommand.CommandText = String.Format("Select {0} from {1} where {2};", columns, Table_name, where);
                        mycommand.ExecuteNonQuery();
                        using (SQLiteDataReader reader = mycommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    for (int i = 0; i < numof_columns; i++)
                                        ret.Add(reader.GetValue(i).ToString());

                                }
                            }

                            reader.Close();
                        }
                    }
                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error somewhere");
            }
            return ret;

        }



    }
}
