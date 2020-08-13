<%@ WebService Language="C#" CodeBehind="WebService1.asmx.cs" Class="RentC.MyWebService.WebService1" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace RentC.MyWebService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string MyMethod()
        {
            string result = "";
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
                        result = oReader[""].ToString();                       
                    }

                    myConnection.Close();
                }               
            }
            return result;
        }
    }
}

