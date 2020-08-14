using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace RentC.MyWebService
{
    public class DatabaseConnection
    {
        public static object[] GetAvailableCars(string sql, string[]columns, object[]param)
        {
            int columnsCount = columns.Length;
            object[]result = new object[columnsCount];
            var con = ConfigurationManager.ConnectionStrings["academy_net"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand(sql, myConnection);
                for(int i = 0; i < param.Length; ++i)
                {
                    oCmd.Parameters.AddWithValue("@" + i.ToString(), param[i]);
                }
                
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        for(int i = 0; i < columnsCount; ++i)
                        {
                            result[i] = oReader[columns[i]];
                        }
                    }

                    myConnection.Close();
                }
            }
            return result;
        }        
    }
}