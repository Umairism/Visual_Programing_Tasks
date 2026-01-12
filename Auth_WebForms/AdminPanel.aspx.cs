using System;
using System.Data;
using System.Web.UI.WebControls;
using Auth_WebForms.DataAccess;

namespace Auth_WebForms
{
    public partial class AdminPanel : System.Web.UI.Page
    {
        private UserDataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = new UserDataAccess();

            if (!IsPostBack)
            {
                // Check if user is in Admin role
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
                // Get all users using ADO.NET Connectionless approach (DataAdapter + DataSet)
                DataSet usersDataSet = _dataAccess.GetAllUsers();

                if (usersDataSet != null && usersDataSet.Tables["Users"].Rows.Count > 0)
                {
                    DataTable usersTable = usersDataSet.Tables["Users"];

                    // Bind to GridView
                    gvUsers.DataSource = usersTable;
                    gvUsers.DataBind();

                    // Calculate statistics using DataTable (disconnected mode)
                    CalculateStatistics(usersTable);
                }
                else
                {
                    gvUsers.DataSource = null;
                    gvUsers.DataBind();
                    ShowMessage("No users found in the system.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading users: " + ex.Message, "danger");
                System.Diagnostics.Debug.WriteLine("Error loading users: " + ex.ToString());
            }
        }

        private void CalculateStatistics(DataTable usersTable)
        {
            try
            {
                int totalUsers = usersTable.Rows.Count;
                int activeUsers = 0;
                int inactiveUsers = 0;
                int lockedUsers = 0;

                // Calculate statistics from DataTable (connectionless)
                foreach (DataRow row in usersTable.Rows)
                {
                    bool isActive = Convert.ToBoolean(row["IsActive"]);
                    bool isLockedOut = Convert.ToBoolean(row["IsLockedOut"]);

                    if (isActive)
                        activeUsers++;
                    else
                        inactiveUsers++;

                    if (isLockedOut)
                        lockedUsers++;
                }

                // Display statistics
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

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(',');
                    int userId = Convert.ToInt32(args[0]);
                    bool currentStatus = Convert.ToBoolean(args[1]);
                    bool newStatus = !currentStatus;

                    // Update user status using connectionless approach
                    bool success = _dataAccess.UpdateUserStatus(userId, newStatus);

                    if (success)
                    {
                        ShowMessage(
                            $"User status updated successfully to {(newStatus ? "Active" : "Inactive")}.",
                            "success"
                        );
                        LoadUsers(); // Refresh the grid
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
            ShowMessage("User list refreshed successfully.", "info");
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = "alert alert-" + type;
        }
    }
}
