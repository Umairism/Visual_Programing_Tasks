using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using StoredProcedure_CRUD.DataAccess;
using StoredProcedure_CRUD.Models;

namespace StoredProcedure_CRUD
{
    public partial class EmployeeList : System.Web.UI.Page
    {
        private EmployeeDataAccess employeeDA = new EmployeeDataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployees();
                LoadStatistics();
                
                // Check for success message from other pages
                if (Request.QueryString["success"] != null)
                {
                    ShowMessage(Request.QueryString["success"], "success");
                }
            }
        }

        private void LoadEmployees()
        {
            try
            {
                List<Employee> employees = employeeDA.GetAllEmployees();
                BindEmployeesGrid(employees);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error loading employees: {ex.Message}", "danger");
            }
        }

        private void LoadStatistics()
        {
            try
            {
                var stats = employeeDA.GetEmployeeStatistics();
                
                lblTotalEmployees.Text = stats["TotalEmployees"].ToString();
                lblActiveEmployees.Text = stats["ActiveEmployees"].ToString();
                lblTotalDepartments.Text = stats["TotalDepartments"].ToString();
                
                if (stats["AverageSalary"] != DBNull.Value)
                {
                    decimal avgSalary = Convert.ToDecimal(stats["AverageSalary"]);
                    lblAverageSalary.Text = avgSalary.ToString("C0");
                }
                else
                {
                    lblAverageSalary.Text = "$0";
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error loading statistics: {ex.Message}", "warning");
            }
        }

        private void BindEmployeesGrid(List<Employee> employees)
        {
            gvEmployees.DataSource = employees;
            gvEmployees.DataBind();
            lblRecordCount.Text = $"{employees.Count} record(s)";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            
            if (string.IsNullOrEmpty(searchTerm))
            {
                ShowMessage("Please enter a search term", "warning");
                return;
            }

            try
            {
                List<Employee> employees = employeeDA.SearchEmployees(searchTerm);
                BindEmployeesGrid(employees);
                
                if (employees.Count == 0)
                {
                    ShowMessage($"No employees found matching '{searchTerm}'", "info");
                }
                else
                {
                    ShowMessage($"Found {employees.Count} employee(s) matching '{searchTerm}'", "success");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error searching employees: {ex.Message}", "danger");
            }
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlFilter.SelectedValue = "All";
            LoadEmployees();
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<Employee> employees;
                
                switch (ddlFilter.SelectedValue)
                {
                    case "Active":
                        employees = employeeDA.GetActiveEmployees();
                        break;
                    case "Inactive":
                        employees = employeeDA.GetAllEmployees();
                        employees = employees.FindAll(emp => !emp.IsActive);
                        break;
                    default:
                        employees = employeeDA.GetAllEmployees();
                        break;
                }
                
                BindEmployeesGrid(employees);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error filtering employees: {ex.Message}", "danger");
            }
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteEmployee")
            {
                try
                {
                    int employeeId = Convert.ToInt32(e.CommandArgument);
                    
                    bool success = employeeDA.DeleteEmployee(employeeId);
                    
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
                catch (Exception ex)
                {
                    ShowMessage($"Error deleting employee: {ex.Message}", "danger");
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
