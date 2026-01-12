using System;
using StoredProcedure_CRUD.DataAccess;
using StoredProcedure_CRUD.Models;

namespace StoredProcedure_CRUD
{
    public partial class EmployeeEdit : System.Web.UI.Page
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
                LoadEmployeeData();
            }
        }

        private void LoadEmployeeData()
        {
            try
            {
                Employee employee = employeeDA.GetEmployeeById(employeeId);

                if (employee == null)
                {
                    ShowMessage("Employee not found", "danger");
                    pnlForm.Visible = false;
                    return;
                }

                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                txtEmail.Text = employee.Email;
                txtPhone.Text = employee.Phone;
                ddlDepartment.SelectedValue = employee.Department;
                txtPosition.Text = employee.Position;
                txtSalary.Text = employee.Salary.ToString("F2");
                txtHireDate.Text = employee.HireDate.ToString("yyyy-MM-dd");
                chkIsActive.Checked = employee.IsActive;
            }
            catch (Exception ex)
            {
                ShowMessage($"Error loading employee: {ex.Message}", "danger");
                pnlForm.Visible = false;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Employee employee = new Employee
                    {
                        EmployeeId = employeeId,
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = string.IsNullOrEmpty(txtPhone.Text.Trim()) ? null : txtPhone.Text.Trim(),
                        Department = ddlDepartment.SelectedValue,
                        Position = txtPosition.Text.Trim(),
                        Salary = decimal.Parse(txtSalary.Text),
                        HireDate = DateTime.Parse(txtHireDate.Text),
                        IsActive = chkIsActive.Checked
                    };

                    bool success = employeeDA.UpdateEmployee(employee);

                    if (success)
                    {
                        Response.Redirect($"EmployeeList.aspx?success=Employee updated successfully!");
                    }
                    else
                    {
                        ShowMessage("Failed to update employee", "danger");
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
