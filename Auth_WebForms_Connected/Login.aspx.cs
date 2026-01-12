using System;
using System.Web;
using System.Web.Security;
using Auth_WebForms_Connected.DataAccess;
using Auth_WebForms_Connected.Helpers;

namespace Auth_WebForms_Connected
{
    public partial class Login : System.Web.UI.Page
    {
        private UserDataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = new UserDataAccess();

            if (!IsPostBack)
            {
                // Check if user is already authenticated
                if (User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Default.aspx");
                }

                // Display logout message if present
                if (Request.QueryString["logout"] == "true")
                {
                    ShowMessage("You have been logged out successfully.", "success");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                // Hash the password using SHA512
                string passwordHash = SecurityHelper.HashPassword(password);

                // Authenticate using ADO.NET Connection-Oriented approach
                // (SqlConnection + SqlCommand + SqlDataReader)
                UserInfo userInfo = _dataAccess.AuthenticateUser(username, passwordHash);

                if (userInfo != null)
                {
                    if (!userInfo.IsActive)
                    {
                        ShowMessage("Your account is inactive. Please contact administrator.", "danger");
                        return;
                    }

                    // Create Forms Authentication ticket
                    bool isPersistent = chkRememberMe.Checked;
                    
                    // Create authentication ticket with user roles
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                                  // version
                        username,                           // username
                        DateTime.Now,                       // issue time
                        DateTime.Now.AddHours(isPersistent ? 24 : 2), // expiration
                        isPersistent,                       // persistent
                        userInfo.Roles,                     // user data (roles)
                        FormsAuthentication.FormsCookiePath // cookie path
                    );

                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create cookie
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    if (isPersistent)
                    {
                        authCookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(authCookie);

                    // Store additional user info in session
                    Session["FullName"] = userInfo.FullName;
                    Session["Email"] = userInfo.Email;
                    Session["Roles"] = userInfo.Roles;
                    Session["Username"] = username;
                    Session["UserId"] = userInfo.UserId;

                    // Log the successful login
                    System.Diagnostics.Debug.WriteLine(
                        $"Connection-Oriented Login: {username} with roles: {userInfo.Roles}"
                    );

                    // Redirect to return URL or default page
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        // Redirect based on role
                        if (userInfo.Roles != null && userInfo.Roles.Contains("Admin"))
                        {
                            Response.Redirect("~/AdminPanel.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/UserDashboard.aspx");
                        }
                    }
                }
                else
                {
                    ShowMessage("Invalid username or password.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Login failed: " + ex.Message, "danger");
                System.Diagnostics.Debug.WriteLine("Login error: " + ex.ToString());
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = "alert alert-" + type;
        }
    }
}
