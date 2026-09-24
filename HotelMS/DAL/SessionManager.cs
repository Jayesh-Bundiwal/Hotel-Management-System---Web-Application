using System;
using System.Web;

namespace HotelMS.DAL
{
    /// <summary>
    /// Tracks the currently logged-in user via ASP.NET Session state.
    /// C# equivalent of the Android app's SessionManager (SharedPreferences).
    /// </summary>
    public static class SessionManager
    {
        private const string KEY_USER_ID = "UserId";
        private const string KEY_USER_NAME = "UserName";

        public static void CreateSession(int userId, string fullName)
        {
            HttpContext.Current.Session[KEY_USER_ID] = userId;
            HttpContext.Current.Session[KEY_USER_NAME] = fullName;
        }

        public static bool IsLoggedIn()
        {
            return HttpContext.Current.Session[KEY_USER_ID] != null;
        }

        public static int GetUserId()
        {
            object v = HttpContext.Current.Session[KEY_USER_ID];
            return v == null ? -1 : (int)v;
        }

        public static string GetUserName()
        {
            object v = HttpContext.Current.Session[KEY_USER_NAME];
            return v == null ? string.Empty : (string)v;
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}
