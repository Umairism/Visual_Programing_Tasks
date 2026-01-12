using System;
using System.Web;
using System.Web.Security;

namespace Auth_WebForms
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is authenticated
                if (User.Identity.IsAuthenticated)
                {
                    LoadUserInfo();
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        private void LoadUserInfo()
        {
            // Get user information from session
            string fullName = Session["FullName"]?.ToString() ?? "User";
            string email = Session["Email"]?.ToString() ?? "";
            string username = Session["Username"]?.ToString() ?? User.Identity.Name;
            string roles = Session["Roles"]?.ToString() ?? "";

            // Display user information
            lblFullName.Text = fullName;
            lblUserName.Text = username;
            lblUsername.Text = username;
            lblEmail.Text = email;
            lblRoles.Text = roles;
            lblUserRole.Text = "Role: " + roles;
            lblLastLogin.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm");

            // Check if user is Admin
            if (User.IsInRole("Admin") || roles.Contains("Admin"))
            {
                pnlAdminAccess.Visible = true;
                liAdminPanel.Visible = true;
                divAdminLink.Visible = true;
            }
            else
            {
                liAdminPanel.Visible = false;
                divAdminLink.Visible = false;
            }
        }
    }
}
