using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ThreeTier_CRUD.BLL;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.Departments
{
    public partial class DepartmentList : System.Web.UI.Page
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                List<Department> departments = departmentBLL.GetAllDepartments();
                gvDepartments.DataSource = departments;
                gvDepartments.DataBind();
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }
}
