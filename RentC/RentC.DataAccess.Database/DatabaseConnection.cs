using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace RentC.DataAccess.Database
{
    public class DatabaseConnection
    {
        public static object GetAvailableCars()
        {
            object result = "";
            var con = ConfigurationManager.ConnectionStrings["academy_net"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "Select Count(CarId) FROM Cars";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        result = oReader[""];
                    }

                    myConnection.Close();
                }
            }
            return result;
        }
    }
}
