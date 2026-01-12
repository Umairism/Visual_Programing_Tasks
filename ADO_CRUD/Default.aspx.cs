using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ADO_CRUD
{
    public partial class Default : System.Web.UI.Page
    {
        // Pure ADO.NET - Get connection string directly
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
            }
        }

        private void LoadStatistics()
        {
            // Direct ADO.NET approach - using SqlConnection and SqlCommand
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                // 1. Create connection
                conn = new SqlConnection(connectionString);

                // 2. Create command
                cmd = new SqlCommand("SELECT * FROM vw_Statistics", conn);
                cmd.CommandType = CommandType.Text;

                // 3. Open connection
                conn.Open();

                // 4. Execute reader
                reader = cmd.ExecuteReader();

                // 5. Read data
                if (reader.Read())
                {
                    lblTotalStudents.Text = reader["TotalStudents"].ToString();
                    lblTotalCourses.Text = reader["TotalCourses"].ToString();
                    lblAverageGPA.Text = Convert.ToDecimal(reader["AverageGPA"]).ToString("F2");
                    lblExcellent.Text = reader["ExcellentStudents"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // 6. Clean up resources - IMPORTANT!
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
