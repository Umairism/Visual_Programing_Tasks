using System;

namespace Auth_WebForms_Connected
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            string fullName = Session["FullName"]?.ToString() ?? "User";
            string email = Session["Email"]?.ToString() ?? "";
            string username = Session["Username"]?.ToString() ?? User.Identity.Name;
            string roles = Session["Roles"]?.ToString() ?? "";

            lblFullName.Text = fullName;
            lblUserName.Text = username;
            lblUsername.Text = username;
            lblEmail.Text = email;
            lblRoles.Text = roles;
            lblUserRole.Text = "Role: " + roles;

            if (User.IsInRole("Admin") || roles.Contains("Admin"))
            {
                pnlAdminAccess.Visible = true;
                liAdminPanel.Visible = true;
            }
            else
            {
                liAdminPanel.Visible = false;
            }
        }
    }
}
