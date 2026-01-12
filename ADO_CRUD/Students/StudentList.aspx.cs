using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ADO_CRUD.Students
{
    public partial class StudentList : System.Web.UI.Page
    {
        // Pure ADO.NET - Direct connection string access
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
                LoadStudents();

                if (Request.QueryString["success"] != null)
                {
                    ShowMessage(Request.QueryString["success"], "success");
                }
            }
        }

        private void LoadCourses()
        {
            // Direct ADO.NET - Using SqlDataAdapter to fill DataTable
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                
                adapter.Fill(dt);

                ddlCourseFilter.Items.Clear();
                ddlCourseFilter.Items.Add(new ListItem("All Courses", "0"));

                foreach (DataRow row in dt.Rows)
                {
                    ddlCourseFilter.Items.Add(new ListItem(row["CourseName"].ToString(), row["CourseId"].ToString()));
                }
            }
        }

        private void LoadStudents()
        {
            // Pure ADO.NET - SqlDataAdapter and DataTable for GridView
            SqlConnection conn = null;
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlConnection(connectionString);
                
                string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                                s.Email, s.GPA, s.IsActive, c.CourseName
                                FROM Students s
                                INNER JOIN Courses c ON s.CourseId = c.CourseId
                                WHERE 1=1 ";

                // Add course filter if selected
                int courseId = Convert.ToInt32(ddlCourseFilter.SelectedValue);
                if (courseId > 0)
                {
                    query += " AND s.CourseId = @CourseId";
                }

                query += " ORDER BY s.StudentId DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                
                if (courseId > 0)
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                }

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading students: " + ex.Message, "danger");
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadStudents();
                return;
            }

            // Direct ADO.NET search with LIKE operator
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                                s.Email, s.GPA, s.IsActive, c.CourseName
                                FROM Students s
                                INNER JOIN Courses c ON s.CourseId = c.CourseId
                                WHERE s.FirstName LIKE @Keyword 
                                   OR s.LastName LIKE @Keyword
                                   OR s.Email LIKE @Keyword
                                   OR s.StudentNumber LIKE @Keyword
                                ORDER BY s.StudentId DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();

                ShowMessage($"Found {dt.Rows.Count} student(s) matching '{keyword}'", "info");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlCourseFilter.SelectedIndex = 0;
            LoadStudents();
            pnlMessage.Visible = false;
        }

        protected void ddlCourseFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteStudent")
            {
                try
                {
                    int studentId = Convert.ToInt32(e.CommandArgument);

                    // Pure ADO.NET DELETE operation
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Students WHERE StudentId = @StudentId";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowMessage("Student deleted successfully!", "success");
                            LoadStudents();
                        }
                        else
                        {
                            ShowMessage("Failed to delete student", "danger");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error: " + ex.Message, "danger");
                }
            }
        }

        protected string GetGPABadgeClass(decimal gpa)
        {
            if (gpa >= 3.75m) return "badge bg-success";
            if (gpa >= 3.50m) return "badge bg-info";
            if (gpa >= 3.00m) return "badge bg-warning";
            return "badge bg-secondary";
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlMessage.Visible = true;
        }
    }
}
