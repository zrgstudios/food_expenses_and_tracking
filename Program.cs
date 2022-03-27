using System;
using System.IO;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

namespace food_expenses_and_tracking
{
    class MySQL_handler
    {
        MySqlConnection conn;
        public static Dictionary<string, string> load_data_from_file()
        {
            string root_path = "E:\\Programming\\projects\\C#\\food_expenses_and_tracking";
            var dotenv = Path.Combine(root_path, "info.env");
            return DotEnv.Load(dotenv);
        }
        public MySqlConnection initialize_conn(Dictionary<string, string> sql_data)
        {
            string server = sql_data["server"];
            string database = sql_data["database"];
            string uid = sql_data["uid"];
            string pass = sql_data["pass"];
            string port = sql_data["port"];
            string ssl = sql_data["ssl"];

            string conn_string = String.Format("server={0};port={4};database={1};Uid={2};Pwd={3};SslMode={5};", 
                                                server, database, uid, pass, port, ssl);

            this.conn = new MySqlConnection(conn_string);
            return this.conn;
        }

        public bool open_conn(MySqlConnection conn)
        {
            try
            {
                conn.Open();
                return true;
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server, contact DB Admin.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again.");
                        break;
                }
                return false;
            }
        }

        public void get_tables(MySqlConnection conn)
        {
            string query = "SHOW FULL TABLES";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader query_result = cmd.ExecuteReader();
            while (query_result.Read())
            {
                Console.WriteLine(query_result[0]);
            }
            query_result.Close();
        }

        public void insert_ingredient_data(MySqlConnection conn)
        {
            DateTime local_date = DateTime.Now;
            string converted_date = local_date.ToString("yyyy-MM-dd");
            Console.WriteLine(converted_date);

            string query = "INSERT INTO ingredient_prices (ingredient_name, ingredient_price, location, date_recorded) VALUES (?ingredient_name, ?ingredient_price, ?location, ?date_recorded);";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.Parameters.Add(new MySqlParameter("ingredient_name", "Gallon Water"));
            command.Parameters.Add(new MySqlParameter("ingredient_price", 0.95));
            command.Parameters.Add(new MySqlParameter("location", "Aldi"));
            command.Parameters.Add(new MySqlParameter("date_recorded", converted_date));
            MySqlDataReader cmd = command.ExecuteReader();
            cmd.Close();

            /*
             Tab for each item (i.e. inserting ingredients, recipes, prices, etc.) 
             
             Make UI to insert ingredients (we will add an auto-incrementer in code, that can be overridden).
                Type in name and insert button?
                2 textboxes, one for name and one for #, and insert button, display inserted foods in grid below?
                    Add a third textbox for it's price? Fourth textbox for location, Fifth checkbox for if we have it stored or not?
             */
        }

        public bool close_connection(MySqlConnection conn)
        {
            try
            {
                conn.Close();
                return true;
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
        
        
    class Program
    {
        static void Main(string[] args)
        {
            MySQL_handler sql_obj = new MySQL_handler();
            
            MySqlConnection conn = sql_obj.initialize_conn(MySQL_handler.load_data_from_file());
            bool conn_check_bool = sql_obj.open_conn(conn);
            if (conn_check_bool)
            {
                sql_obj.get_tables(conn);
                sql_obj.insert_ingredient_data(conn);
                sql_obj.close_connection(conn);
            }
        }
    }
}
