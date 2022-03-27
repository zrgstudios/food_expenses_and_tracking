﻿using System;
using System.IO;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

namespace food_expenses_and_tracking
{
    class MySQL_handler
    {
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

            MySqlConnection conn = new MySqlConnection(conn_string);
            return conn;
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
                sql_obj.close_connection(conn);
            }
        }
    }
}
