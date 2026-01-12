using System;
using StoredProcedure_CRUD.DataAccess;
using StoredProcedure_CRUD.Models;

namespace StoredProcedure_CRUD
{
    public partial class EmployeeAdd : System.Web.UI.Page
    {
        private EmployeeDataAccess employeeDA = new EmployeeDataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default hire date to today
                txtHireDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Employee employee = new Employee
                    {
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = string.IsNullOrEmpty(txtPhone.Text.Trim()) ? null : txtPhone.Text.Trim(),
                        Department = ddlDepartment.SelectedValue,
                        Position = txtPosition.Text.Trim(),
                        Salary = decimal.Parse(txtSalary.Text),
                        HireDate = DateTime.Parse(txtHireDate.Text)
                    };

                    int newEmployeeId = employeeDA.InsertEmployee(employee);

                    if (newEmployeeId > 0)
                    {
                        // Redirect to list page with success message
                        Response.Redirect($"EmployeeList.aspx?success=Employee added successfully! (ID: {newEmployeeId})");
                    }
                    else
                    {
                        ShowMessage("Failed to add employee", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error: {ex.Message}", "danger");
                }
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = $"alert alert-{type} alert-dismissible fade show";
        }
    }
}
