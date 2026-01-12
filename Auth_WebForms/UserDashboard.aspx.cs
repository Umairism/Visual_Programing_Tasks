using System;
using System.Web;

namespace Auth_WebForms
{
    public partial class UserDashboard : System.Web.UI.Page
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
            lblFullNameProfile.Text = fullName;
            lblNavUsername.Text = username;
            lblUsername.Text = username;
            lblEmail.Text = email;
            lblRoles.Text = roles;
            lblLastLogin.Text = DateTime.Now.ToString("MMMM dd, yyyy at HH:mm");

            // Check if user is Admin
            if (User.IsInRole("Admin") || roles.Contains("Admin"))
            {
                liAdminPanel.Visible = true;
                divAdminAction.Visible = true;
                liAdminFeature.Visible = true;
            }
            else
            {
                liAdminPanel.Visible = false;
                divAdminAction.Visible = false;
                liAdminFeature.Visible = false;
            }
        }
    }
}
