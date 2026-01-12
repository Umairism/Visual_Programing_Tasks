using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ADO_WinForms.Forms
{
    public partial class CourseListForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;
        private DataGridView dgvCourses;

        public CourseListForm()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.Text = "Course List - ADO.NET Windows Forms";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Title Panel
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(142, 68, 173)
            };

            Label titleLabel = new Label
            {
                Text = "Course Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 25)
            };

            titlePanel.Controls.Add(titleLabel);

            // DataGridView
            dgvCourses = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(940, 400),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            // Close Button
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(860, 515),
                Size = new Size(100, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titlePanel, dgvCourses, btnClose });
        }

        private void LoadCourses()
        {
            // Pure ADO.NET - Load courses with student count
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT c.CourseId, c.CourseCode, c.CourseName, c.Credits, 
                                    c.Department, 
                                    COUNT(s.StudentId) AS EnrolledStudents,
                                    CASE WHEN c.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
                                    FROM Courses c
                                    LEFT JOIN Students s ON c.CourseId = s.CourseId AND s.IsActive = 1
                                    GROUP BY c.CourseId, c.CourseCode, c.CourseName, c.Credits, c.Department, c.IsActive
                                    ORDER BY c.CourseName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvCourses.DataSource = dt;

                    // Style columns
                    if (dgvCourses.Columns.Count > 0)
                    {
                        dgvCourses.Columns["CourseId"].HeaderText = "ID";
                        dgvCourses.Columns["CourseId"].Width = 50;
                        dgvCourses.Columns["CourseCode"].HeaderText = "Code";
                        dgvCourses.Columns["CourseCode"].Width = 100;
                        dgvCourses.Columns["CourseName"].HeaderText = "Course Name";
                        dgvCourses.Columns["Credits"].HeaderText = "Credits";
                        dgvCourses.Columns["Credits"].Width = 80;
                        dgvCourses.Columns["Department"].HeaderText = "Department";
                        dgvCourses.Columns["EnrolledStudents"].HeaderText = "Students";
                        dgvCourses.Columns["EnrolledStudents"].Width = 100;
                        dgvCourses.Columns["Status"].HeaderText = "Status";
                        dgvCourses.Columns["Status"].Width = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
