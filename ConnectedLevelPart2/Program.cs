using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;

namespace ConnectedLevelPart2
{
    class Program
    {
        static void Main(string[] args)
        {
            DbProviderFactory factory = DbProviderFactories
                .GetFactory(ConfigurationManager
                .ConnectionStrings["ConnectionString"]
                .ProviderName);

            using (var connection = factory.CreateConnection()) 
            using (var command = factory.CreateCommand())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                connection.Open();

                command.CommandText = "insert into users (id, login, password, updateDateTime)" +
                                   "values (@id, @login, @password, @updateDateTime)";

                var idParametr = factory.CreateParameter();
                idParametr.ParameterName = "@id";
                idParametr.DbType = System.Data.DbType.Int32;
                idParametr.Value = 9;
                command.Parameters.Add(idParametr);

                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@id",
                //    SqlDbType = System.Data.SqlDbType.Int,
                //    Value = 8,
                //    IsNullable = false
                //});

                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@login",
                //    SqlDbType = System.Data.SqlDbType.NVarChar,
                //    Value = "Di",
                //    IsNullable = false
                //});

                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@password",
                //    SqlDbType = System.Data.SqlDbType.NVarChar,
                //    Value = "Di"
                //});

                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@updateDateTime",
                //    SqlDbType = System.Data.SqlDbType.DateTime2,
                //    Value = DateTime.Now
                //});

                command.Connection = connection;

                ExecuteTransaction(connection, command);

            }
        }

        public static void ExecuteTransaction(DbConnection connection, params DbCommand[] commands)
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
                catch (DbException exepction)
                {
                    Console.WriteLine(exepction.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
