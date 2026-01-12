using System;
using System.Data;
using System.Web;
using System.Web.Security;
using Auth_WebForms.DataAccess;
using Auth_WebForms.Helpers;

namespace Auth_WebForms
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

                // Authenticate using ADO.NET Connectionless approach (DataAdapter + DataSet)
                DataTable userTable = _dataAccess.AuthenticateUser(username, passwordHash);

                if (userTable != null && userTable.Rows.Count > 0)
                {
                    DataRow userRow = userTable.Rows[0];

                    // Get user details
                    string fullName = userRow["FullName"].ToString();
                    string email = userRow["Email"].ToString();
                    string roles = userRow["Roles"].ToString();
                    bool isActive = Convert.ToBoolean(userRow["IsActive"]);

                    if (!isActive)
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
                        roles,                              // user data (roles)
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
                    Session["FullName"] = fullName;
                    Session["Email"] = email;
                    Session["Roles"] = roles;
                    Session["Username"] = username;

                    // Log the successful login
                    System.Diagnostics.Debug.WriteLine($"User logged in: {username} with roles: {roles}");

                    // Redirect to return URL or default page
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        // Redirect based on role
                        if (roles.Contains("Admin"))
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
