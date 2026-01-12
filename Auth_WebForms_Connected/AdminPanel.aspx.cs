using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Auth_WebForms_Connected.DataAccess;

namespace Auth_WebForms_Connected
{
    public partial class AdminPanel : System.Web.UI.Page
    {
        private UserDataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = new UserDataAccess();

            if (!IsPostBack)
            {
                if (!User.IsInRole("Admin"))
                {
                    Response.Redirect("~/Default.aspx");
                }

                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            try
            {
                // Get all users using Connection-Oriented approach (SqlDataReader)
                List<UserInfo> users = _dataAccess.GetAllUsers();

                if (users != null && users.Count > 0)
                {
                    // Bind to Repeater control
                    rptUsers.DataSource = users;
                    rptUsers.DataBind();

                    // Calculate statistics
                    CalculateStatistics(users);
                }
                else
                {
                    rptUsers.DataSource = null;
                    rptUsers.DataBind();
                    ShowMessage("No users found in the system.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading users: " + ex.Message, "danger");
                System.Diagnostics.Debug.WriteLine("Error loading users: " + ex.ToString());
            }
        }

        private void CalculateStatistics(List<UserInfo> users)
        {
            try
            {
                int totalUsers = users.Count;
                int activeUsers = 0;
                int inactiveUsers = 0;
                int lockedUsers = 0;

                foreach (UserInfo user in users)
                {
                    if (user.IsActive)
                        activeUsers++;
                    else
                        inactiveUsers++;

                    if (user.IsLockedOut)
                        lockedUsers++;
                }

                lblTotalUsers.Text = totalUsers.ToString();
                lblActiveUsers.Text = activeUsers.ToString();
                lblInactiveUsers.Text = inactiveUsers.ToString();
                lblLockedUsers.Text = lockedUsers.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error calculating statistics: " + ex.ToString());
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(',');
                    int userId = Convert.ToInt32(args[0]);
                    bool currentStatus = Convert.ToBoolean(args[1]);
                    bool newStatus = !currentStatus;

                    // Update user status using connection-oriented approach
                    bool success = _dataAccess.UpdateUserStatus(userId, newStatus);

                    if (success)
                    {
                        ShowMessage(
                            $"User status updated successfully to {(newStatus ? "Active" : "Inactive")}.",
                            "success"
                        );
                        LoadUsers(); // Refresh the list
                    }
                    else
                    {
                        ShowMessage("Failed to update user status.", "warning");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error updating user status: " + ex.Message, "danger");
                    System.Diagnostics.Debug.WriteLine("Error updating user status: " + ex.ToString());
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
            ShowMessage("User list refreshed successfully using SqlDataReader.", "info");
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = "alert alert-" + type;
        }
    }
}
