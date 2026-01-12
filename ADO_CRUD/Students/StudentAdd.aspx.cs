using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ADO_CRUD.Students
{
    public partial class StudentAdd : System.Web.UI.Page
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
            // Direct ADO.NET - SqlDataReader for dropdown
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand("SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName", conn);
                
                conn.Open();
                reader = cmd.ExecuteReader();

                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-- Select Course --", "0"));

                while (reader.Read())
                {
                    ddlCourse.Items.Add(new ListItem(reader["CourseName"].ToString(), reader["CourseId"].ToString()));
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading courses: " + ex.Message, "danger");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Pure ADO.NET INSERT with SqlCommand and SqlParameters
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(connectionString);
                
                string query = @"INSERT INTO Students 
                                (StudentNumber, FirstName, LastName, Email, DateOfBirth, CourseId, GPA, IsActive, EnrollmentDate, CreatedDate)
                                VALUES 
                                (@StudentNumber, @FirstName, @LastName, @Email, @DateOfBirth, @CourseId, @GPA, @IsActive, GETDATE(), GETDATE());
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;

                // Add parameters - preventing SQL injection
                cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text));
                cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(ddlCourse.SelectedValue));
                cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
                cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                conn.Open();
                
                // ExecuteScalar returns the new ID
                int newStudentId = Convert.ToInt32(cmd.ExecuteScalar());

                Response.Redirect($"StudentList.aspx?success=Student added successfully! (ID: {newStudentId})");
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL-specific errors
                if (sqlEx.Number == 2627) // Unique constraint violation
                {
                    ShowMessage("Error: Student number already exists!", "danger");
                }
                else
                {
                    ShowMessage("Database Error: " + sqlEx.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.CssClass = $"alert alert-{type}";
            pnlMessage.Controls.Clear();
            pnlMessage.Controls.Add(new System.Web.UI.LiteralControl(message));
            pnlMessage.Visible = true;
        }
    }
}
