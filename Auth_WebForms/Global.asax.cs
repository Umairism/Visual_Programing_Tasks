using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Auth_WebForms
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Get the Forms Authentication ticket from the cookie
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                try
                {
                    // Decrypt the authentication ticket
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null && !authTicket.Expired)
                    {
                        // Get the roles from the ticket UserData
                        string[] roles = authTicket.UserData.Split(',');

                        // Create a GenericIdentity with the username
                        GenericIdentity identity = new GenericIdentity(authTicket.Name, "Forms");

                        // Create a GenericPrincipal with the identity and roles
                        GenericPrincipal principal = new GenericPrincipal(identity, roles);

                        // Set the principal for the current request
                        Context.User = principal;

                        // Also set it for the thread
                        System.Threading.Thread.CurrentPrincipal = principal;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine("Error in Application_AuthenticateRequest: " + ex.Message);
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the last error
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Application Error: " + exception.ToString());

                // Clear the error
                Server.ClearError();

                // Redirect to error page (optional)
                // Response.Redirect("~/Error.aspx");
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }
    }
}
