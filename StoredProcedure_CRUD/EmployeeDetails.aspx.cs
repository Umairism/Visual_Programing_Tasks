using System;
using StoredProcedure_CRUD.DataAccess;
using StoredProcedure_CRUD.Models;

namespace StoredProcedure_CRUD
{
    public partial class EmployeeDetails : System.Web.UI.Page
    {
        private EmployeeDataAccess employeeDA = new EmployeeDataAccess();
        private int employeeId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out employeeId))
            {
                Response.Redirect("EmployeeList.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEmployeeDetails();
            }
        }

        private void LoadEmployeeDetails()
        {
            try
            {
                Employee employee = employeeDA.GetEmployeeById(employeeId);

                if (employee == null)
                {
                    pnlDetails.Visible = false;
                    pnlMessage.Visible = true;
                    lblMessage.Text = "Employee not found";
                    return;
                }

                // Display employee details
                lblInitials.Text = employee.FirstName.Substring(0, 1) + employee.LastName.Substring(0, 1);
                lblFullName.Text = employee.FullName;
                lblEmployeeId.Text = employee.EmployeeId.ToString();
                lblEmail.Text = employee.Email;
                lblPhone.Text = employee.Phone ?? "N/A";
                lblDepartment.Text = employee.Department;
                lblPosition.Text = employee.Position;
                lblSalary.Text = employee.FormattedSalary;
                lblHireDate.Text = employee.FormattedHireDate;
                lblCreatedDate.Text = employee.CreatedDate.ToString("MMM dd, yyyy hh:mm tt");
                lblModifiedDate.Text = employee.ModifiedDate?.ToString("MMM dd, yyyy hh:mm tt") ?? "Never";
                
                // Status badge
                lblStatus.Text = employee.Status;
                pnlStatus.CssClass = employee.IsActive ? "badge bg-success" : "badge bg-danger";

                // Set edit link
                hlEdit.NavigateUrl = $"EmployeeEdit.aspx?id={employee.EmployeeId}";
            }
            catch (Exception ex)
            {
                pnlDetails.Visible = false;
                pnlMessage.Visible = true;
                lblMessage.Text = $"Error loading employee details: {ex.Message}";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = employeeDA.DeleteEmployee(employeeId);

                if (success)
                {
                    Response.Redirect("EmployeeList.aspx?success=Employee deleted successfully!");
                }
                else
                {
                    pnlMessage.Visible = true;
                    lblMessage.Text = "Failed to delete employee";
                }
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                lblMessage.Text = $"Error deleting employee: {ex.Message}";
            }
        }
    }
}
