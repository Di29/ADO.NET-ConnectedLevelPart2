using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace ConnectedLevelPart2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();

                command.CommandText = "insert into users (id, login, password, updateDateTime)" +
                                   "values (@id, @login, @password, @updateDateTime)";

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = 8,
                    IsNullable = false
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@login",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Value = "Di",
                    IsNullable = false
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@password",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Value = "Di"
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@updateDateTime",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                    Value = DateTime.Now
                });

                command.Connection = connection;

                ExecuteTransaction(connection, command);

            }
        }

        public static void ExecuteTransaction(SqlConnection connection, params SqlCommand[] commands)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach(var command in commands)
                    {
                        command.Transaction = transaction;
                    }
                    foreach(var command in commands)
                    {
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (SqlException exepction)
                {
                    Console.WriteLine(exepction.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
