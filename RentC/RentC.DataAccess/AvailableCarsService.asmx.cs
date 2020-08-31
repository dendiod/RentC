using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
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
        public QueryCar[] GetAvailableCars(string orderBy, QueryCar car)
        {
            ModelContext modelContext = new ModelContext();
            QueryManager queryManager = new QueryManager(new SQLRepo<Car>(modelContext), new SQLRepo<Customer>(modelContext),
                new SQLRepo<Reservation>(modelContext), new SQLRepo<Location>(modelContext), 
                new SQLRepo<Model>(modelContext), new SQLRepo<Manufacturer>(modelContext));

            return queryManager.GetAvailableCars(orderBy, car);
        }
    }
}
