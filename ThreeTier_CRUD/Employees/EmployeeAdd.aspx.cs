using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ThreeTier_CRUD.BLL;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.Employees
{
    public partial class EmployeeAdd : System.Web.UI.Page
    {
        // PRESENTATION LAYER - Only calls BLL
        private EmployeeBLL employeeBLL = new EmployeeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                txtHireDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void LoadDepartments()
        {
            try
            {
                // Call BLL to get active departments
                List<Department> departments = departmentBLL.GetActiveDepartments();
                
                ddlDepartment.Items.Clear();
                ddlDepartment.Items.Add(new ListItem("-- Select Department --", "0"));
                
                foreach (Department dept in departments)
                {
                    ddlDepartment.Items.Add(new ListItem(dept.DisplayName, dept.DepartmentId.ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading departments: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Create employee object
                Employee employee = new Employee
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    DepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue),
                    Position = txtPosition.Text.Trim(),
                    Salary = Convert.ToDecimal(txtSalary.Text),
                    HireDate = Convert.ToDateTime(txtHireDate.Text),
                    IsActive = chkIsActive.Checked
                };

                // Call BLL to add employee (BLL handles all validation)
                int newEmployeeId = employeeBLL.AddEmployee(employee);

                // Redirect with success message
                Response.Redirect($"EmployeeList.aspx?success=Employee added successfully! (ID: {newEmployeeId})");
            }
            catch (ValidationException vex)
            {
                // Business validation error from BLL
                ShowMessage(vex.Message, "warning");
            }
            catch (Exception ex)
            {
                ShowMessage("Error adding employee: " + ex.Message, "danger");
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.CssClass = $"alert alert-{type}";
            pnlMessage.Controls.Clear();
            pnlMessage.Controls.Add(new LiteralControl(message));
            pnlMessage.Visible = true;
        }
    }
}
