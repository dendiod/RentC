using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using RentC.DataAccess.Models.QueryModels;

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
        public QueryCar[] GetAvailableCars(bool asc, string orderBy)
        {
            QueryManager queryManager = new QueryManager(new ModelContext());
            return queryManager.GetAvailableCars(asc, orderBy);
        }
    }
}
