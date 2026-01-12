using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ADO_WinForms.Forms
{
    public partial class MainForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        public MainForm()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.Text = "Student Management System - Pure ADO.NET";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Title Panel
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label titleLabel = new Label
            {
                Text = "Student Management System",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            Label subtitleLabel = new Label
            {
                Text = "Pure ADO.NET with Windows Forms",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(236, 240, 241),
                AutoSize = true,
                Location = new Point(20, 60)
            };

            titlePanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });

            // Statistics Panel
            Panel statsPanel = new Panel
            {
                Location = new Point(20, 120),
                Size = new Size(940, 120),
                BackColor = Color.White
            };

            // Statistics Labels
            var totalStudentsStats = CreateStatLabel("0", "Total Students", 20, Color.FromArgb(41, 128, 185));
            var totalCoursesStats = CreateStatLabel("0", "Total Courses", 255, Color.FromArgb(142, 68, 173));
            var averageGPAStats = CreateStatLabel("0.00", "Average GPA", 490, Color.FromArgb(39, 174, 96));
            var excellentStudentsStats = CreateStatLabel("0", "Excellent (≥ 3.75)", 725, Color.FromArgb(230, 126, 34));

            lblTotalStudents = totalStudentsStats.Label;
            lblTotalCourses = totalCoursesStats.Label;
            lblAverageGPA = averageGPAStats.Label;
            lblExcellentStudents = excellentStudentsStats.Label;

            statsPanel.Controls.AddRange(new Control[] {
                totalStudentsStats.Label, totalStudentsStats.Caption,
                totalCoursesStats.Label, totalCoursesStats.Caption,
                averageGPAStats.Label, averageGPAStats.Caption,
                excellentStudentsStats.Label, excellentStudentsStats.Caption
            });

            // Navigation Panel
            Panel navPanel = new Panel
            {
                Location = new Point(20, 260),
                Size = new Size(940, 350),
                BackColor = Color.White
            };

            Label navTitle = new Label
            {
                Text = "Navigation",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            Button btnStudents = CreateMenuButton("Manage Students", "View, Add, Edit, and Delete student records", 20, 70, Color.FromArgb(41, 128, 185));
            Button btnCourses = CreateMenuButton("Manage Courses", "View course information and enrollment", 490, 70, Color.FromArgb(142, 68, 173));
            Button btnRefresh = CreateMenuButton("Refresh Statistics", "Reload dashboard statistics", 20, 190, Color.FromArgb(39, 174, 96));
            Button btnExit = CreateMenuButton("Exit Application", "Close the application", 490, 190, Color.FromArgb(231, 76, 60));

            btnStudents.Click += BtnStudents_Click;
            btnCourses.Click += BtnCourses_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnExit.Click += (s, e) => Application.Exit();

            navPanel.Controls.AddRange(new Control[] { navTitle, btnStudents, btnCourses, btnRefresh, btnExit });

            // Footer Label
            Label footerLabel = new Label
            {
                Text = "Pure ADO.NET - Direct SqlConnection, SqlCommand, SqlDataReader usage",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(20, 625),
                AutoSize = true
            };

            this.Controls.AddRange(new Control[] { titlePanel, statsPanel, navPanel, footerLabel });
        }

        private (Label Label, Label Caption) CreateStatLabel(string value, string caption, int x, Color color)
        {
            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(x, 20),
                AutoSize = true
            };

            Label captionLabel = new Label
            {
                Text = caption,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(x, 75),
                AutoSize = true
            };

            return (valueLabel, captionLabel);
        }

        private Button CreateMenuButton(string title, string description, int x, int y, Color color)
        {
            Button btn = new Button
            {
                Size = new Size(450, 100),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Text = title + "\n" + description,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Label lblTotalStudents;
        private Label lblTotalCourses;
        private Label lblAverageGPA;
        private Label lblExcellentStudents;

        private void LoadStatistics()
        {
            // Pure ADO.NET - SqlConnection and SqlDataReader
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand("SELECT * FROM vw_Statistics", conn);
                cmd.CommandType = CommandType.Text;

                conn.Open();
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTotalStudents.Text = reader["TotalStudents"].ToString();
                    lblTotalCourses.Text = reader["TotalCourses"].ToString();
                    lblAverageGPA.Text = Convert.ToDecimal(reader["AverageGPA"]).ToString("F2");
                    lblExcellentStudents.Text = reader["ExcellentStudents"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void BtnStudents_Click(object sender, EventArgs e)
        {
            StudentListForm studentForm = new StudentListForm();
            studentForm.ShowDialog();
            LoadStatistics(); // Refresh after returning
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            CourseListForm courseForm = new CourseListForm();
            courseForm.ShowDialog();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            MessageBox.Show("Statistics refreshed successfully!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
