using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using RentC.Core.Models.QueryModels;

namespace RentC.DataAccess
{
    /// <summary>
    /// Summary description for AvailableCarsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AvailableCarsService : System.Web.Services.WebService
    {

        [WebMethod]
        public List<QueryCar> GetAvailableCars(bool asc, string orderBy)
        {
            return Data<int>.GetAvailableCars(asc, orderBy);
        }
    }
}
