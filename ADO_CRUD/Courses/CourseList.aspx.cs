using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ADO_CRUD.Courses
{
    public partial class CourseList : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            // Pure ADO.NET approach - SqlDataAdapter fills DataTable
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT c.CourseId, c.CourseCode, c.CourseName, c.Credits, 
                                c.Department, c.IsActive,
                                COUNT(s.StudentId) AS StudentCount
                                FROM Courses c
                                LEFT JOIN Students s ON c.CourseId = s.CourseId AND s.IsActive = 1
                                GROUP BY c.CourseId, c.CourseCode, c.CourseName, c.Credits, c.Department, c.IsActive
                                ORDER BY c.CourseName";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                
                adapter.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }
    }
}
