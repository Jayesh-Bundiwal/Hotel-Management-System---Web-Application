using System;
using HotelMS.DAL;

namespace HotelMS
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Creates the HotelMSDb database and all tables on LocalDB the
            // first time the app runs, and seeds the four sample room types
            // (Standard, Deluxe, Suite, Executive Suite) - same behaviour as
            // the Android app's SQLiteOpenHelper.onCreate().
            DatabaseHelper.EnsureDatabase();
        }
    }
}
