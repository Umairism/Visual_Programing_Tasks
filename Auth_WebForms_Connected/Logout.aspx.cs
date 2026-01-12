using System;
using System.Web;
using System.Web.Security;

namespace Auth_WebForms_Connected
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Sign out from Forms Authentication
            FormsAuthentication.SignOut();

            // Clear authentication cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);

            // Redirect to login page with logout message
            Response.Redirect("~/Login.aspx?logout=true", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
