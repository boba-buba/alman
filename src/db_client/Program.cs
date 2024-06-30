using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System;

namespace db_client
{
    internal class Program
    {

        private static void CreateCommand(string query, string connectionString)
        {

            using (var connection = new SqlConnection(connectionString))
            {
               
                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                connection.Open();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", result[0], result[1], result[2], result[3], result[4]));
                    }
                }
            }

        }
        static void Main(string[] args)
        {
            string databaseName = "alman_db";

            string connectionString = "";
            string command = "SELECT * from Alman_db.Child";

            CreateCommand(command, connectionString);
        }
    }
}
//To save non ASCII code
// insert into [alman_db].[Child] (ChildLastName, ChildFirstName, ContractType, ChildGroup) values (N'пщупп', N'ппво', N'щлиыы', 1);
