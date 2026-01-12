using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ADO_CRUD.Students
{
    public partial class StudentEdit : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
                LoadStudentData();
            }
        }

        private void LoadCourses()
        {
            // Direct ADO.NET approach
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName", conn);
                
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-- Select Course --", "0"));

                while (reader.Read())
                {
                    ddlCourse.Items.Add(new ListItem(reader["CourseName"].ToString(), reader["CourseId"].ToString()));
                }

                reader.Close();
            }
        }

        private void LoadStudentData()
        {
            int studentId = 0;
            if (Request.QueryString["id"] != null)
            {
                studentId = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                Response.Redirect("StudentList.aspx");
                return;
            }

            // Pure ADO.NET - SqlDataReader to load student data
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand("SELECT * FROM Students WHERE StudentId = @StudentId", conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtStudentId.Text = reader["StudentId"].ToString();
                    txtStudentNumber.Text = reader["StudentNumber"].ToString();
                    txtFirstName.Text = reader["FirstName"].ToString();
                    txtLastName.Text = reader["LastName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtDateOfBirth.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                    txtGPA.Text = reader["GPA"].ToString();
                    ddlCourse.SelectedValue = reader["CourseId"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);
                }
                else
                {
                    Response.Redirect("StudentList.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading student: " + ex.Message, "danger");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Direct ADO.NET UPDATE operation
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(connectionString);
                
                string query = @"UPDATE Students 
                                SET StudentNumber = @StudentNumber,
                                    FirstName = @FirstName,
                                    LastName = @LastName,
                                    Email = @Email,
                                    DateOfBirth = @DateOfBirth,
                                    CourseId = @CourseId,
                                    GPA = @GPA,
                                    IsActive = @IsActive,
                                    ModifiedDate = GETDATE()
                                WHERE StudentId = @StudentId";

                cmd = new SqlCommand(query, conn);

                // Add parameters manually
                cmd.Parameters.AddWithValue("@StudentId", Convert.ToInt32(txtStudentId.Text));
                cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text));
                cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(ddlCourse.SelectedValue));
                cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
                cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                conn.Open();
                
                // ExecuteNonQuery returns number of rows affected
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Response.Redirect("StudentList.aspx?success=Student updated successfully!");
                }
                else
                {
                    ShowMessage("Failed to update student", "danger");
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
