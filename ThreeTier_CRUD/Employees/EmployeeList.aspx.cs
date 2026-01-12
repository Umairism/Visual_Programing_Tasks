using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ThreeTier_CRUD.BLL;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.Employees
{
    public partial class EmployeeList : System.Web.UI.Page
    {
        // PRESENTATION LAYER - ONLY calls BLL, NEVER calls DAL directly
        private EmployeeBLL employeeBLL = new EmployeeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
                LoadEmployees();
                
                // Check for success message
                if (Request.QueryString["success"] != null)
                {
                    ShowMessage(Request.QueryString["success"], "success");
                }
            }
        }

        private void LoadStatistics()
        {
            try
            {
                // Call BLL to get statistics
                Dictionary<string, object> stats = employeeBLL.GetEmployeeStatistics();
                
                lblTotalEmployees.Text = stats["TotalEmployees"].ToString();
                lblActiveEmployees.Text = stats["ActiveEmployees"].ToString();
                lblAvgSalary.Text = ((decimal)stats["AverageSalary"]).ToString("C0");
                
                // Get department count from Department BLL
                List<Department> departments = departmentBLL.GetAllDepartments();
                lblDepartmentCount.Text = departments.Count.ToString();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading statistics: " + ex.Message, "danger");
            }
        }

        private void LoadEmployees()
        {
            try
            {
                List<Employee> employees;
                
                string filter = ddlFilter.SelectedValue;
                
                if (filter == "Active")
                {
                    employees = employeeBLL.GetActiveEmployees();
                }
                else if (filter == "Inactive")
                {
                    employees = employeeBLL.GetAllEmployees().Where(e => !e.IsActive).ToList();
                }
                else
                {
                    employees = employeeBLL.GetAllEmployees();
                }
                
                gvEmployees.DataSource = employees;
                gvEmployees.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading employees: " + ex.Message, "danger");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                LoadEmployees();
                return;
            }
            
            try
            {
                // Call BLL for search
                List<Employee> employees = employeeBLL.SearchEmployees(keyword);
                gvEmployees.DataSource = employees;
                gvEmployees.DataBind();
                
                ShowMessage($"Found {employees.Count} employee(s) matching '{keyword}'", "info");
            }
            catch (Exception ex)
            {
                ShowMessage("Error searching employees: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlFilter.SelectedValue = "Active";
            LoadEmployees();
            LoadStatistics();
            pnlMessage.Visible = false;
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteEmployee")
            {
                try
                {
                    int employeeId = Convert.ToInt32(e.CommandArgument);
                    
                    // Call BLL to delete employee
                    bool success = employeeBLL.DeleteEmployee(employeeId);
                    
                    if (success)
                    {
                        ShowMessage("Employee deleted successfully!", "success");
                        LoadEmployees();
                        LoadStatistics();
                    }
                    else
                    {
                        ShowMessage("Failed to delete employee", "danger");
                    }
                }
                catch (ValidationException vex)
                {
                    ShowMessage(vex.Message, "warning");
                }
                catch (Exception ex)
                {
                    ShowMessage("Error deleting employee: " + ex.Message, "danger");
                }
            }
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlMessage.Visible = true;
        }
    }
}
