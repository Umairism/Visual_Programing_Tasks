using System;
using Auth_WebForms.DataAccess;
using Auth_WebForms.Helpers;

namespace Auth_WebForms
{
    public partial class Register : System.Web.UI.Page
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
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;
                string fullName = txtFullName.Text.Trim();
                string roleName = ddlRole.SelectedValue;

                // Validate password strength
                if (!SecurityHelper.IsPasswordStrong(password))
                {
                    string message = SecurityHelper.GetPasswordStrengthMessage(password);
                    ShowMessage(message, "warning");
                    return;
                }

                // Hash the password using SHA512
                string passwordHash = SecurityHelper.HashPassword(password);

                // Register user using ADO.NET Connectionless approach (DataAdapter + DataSet)
                int userId = _dataAccess.RegisterUser(username, email, passwordHash, fullName, roleName);

                if (userId > 0)
                {
                    ShowMessage(
                        "Account created successfully! You can now login with your credentials.", 
                        "success"
                    );

                    // Clear form
                    ClearForm();

                    // Redirect to login after 2 seconds
                    Response.AddHeader("REFRESH", "2;URL=Login.aspx");
                }
                else
                {
                    ShowMessage("Registration failed. Please try again.", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Registration failed: " + ex.Message, "danger");
                System.Diagnostics.Debug.WriteLine("Registration error: " + ex.ToString());
            }
        }

        protected void cvAgree_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = chkAgree.Checked;
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = "alert alert-" + type;
        }

        private void ClearForm()
        {
            txtUsername.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtFullName.Text = string.Empty;
            chkAgree.Checked = false;
            ddlRole.SelectedIndex = 0;
        }
    }
}
