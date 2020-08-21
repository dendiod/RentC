using RentC.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    class Program
    {                
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");
            InAppBehavior inAppBehavior = new InAppBehavior();
            inAppBehavior.WelcommmingScreen();
        }
    }
}
