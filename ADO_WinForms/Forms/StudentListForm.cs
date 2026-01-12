using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ADO_WinForms.Forms
{
    public partial class StudentListForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;
        private DataGridView dgvStudents;
        private TextBox txtSearch;
        private ComboBox cmbCourseFilter;

        public StudentListForm()
        {
            InitializeComponent();
            LoadCourses();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.Text = "Student List - ADO.NET Windows Forms";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Title Panel
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label titleLabel = new Label
            {
                Text = "Student Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 25)
            };

            titlePanel.Controls.Add(titleLabel);

            // Search Panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(1140, 60),
                BackColor = Color.White
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Location = new Point(80, 17),
                Size = new Size(300, 25)
            };

            Button btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(390, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;

            Button btnReset = new Button
            {
                Text = "Reset",
                Location = new Point(480, 15),
                Size = new Size(80, 30),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += BtnReset_Click;

            Label lblFilter = new Label
            {
                Text = "Filter by Course:",
                Location = new Point(600, 20),
                AutoSize = true
            };

            cmbCourseFilter = new ComboBox
            {
                Location = new Point(710, 17),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCourseFilter.SelectedIndexChanged += CmbCourseFilter_SelectedIndexChanged;

            Button btnAdd = new Button
            {
                Text = "➕ Add Student",
                Location = new Point(1000, 15),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.Click += BtnAdd_Click;

            searchPanel.Controls.AddRange(new Control[] {
                lblSearch, txtSearch, btnSearch, btnReset, lblFilter, cmbCourseFilter, btnAdd
            });

            // DataGridView
            dgvStudents = new DataGridView
            {
                Location = new Point(20, 180),
                Size = new Size(1140, 420),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            // Add action buttons column
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
            {
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 80
            };

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Width = 80
            };

            dgvStudents.CellContentClick += DgvStudents_CellContentClick;

            // Close Button
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(1060, 610),
                Size = new Size(100, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titlePanel, searchPanel, dgvStudents, btnClose });
        }

        private void LoadCourses()
        {
            // Pure ADO.NET - Loading courses for filter
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                cmbCourseFilter.Items.Clear();
                cmbCourseFilter.Items.Add(new ComboBoxItem { Text = "All Courses", Value = 0 });

                while (reader.Read())
                {
                    cmbCourseFilter.Items.Add(new ComboBoxItem
                    {
                        Text = reader["CourseName"].ToString(),
                        Value = Convert.ToInt32(reader["CourseId"])
                    });
                }

                cmbCourseFilter.DisplayMember = "Text";
                cmbCourseFilter.ValueMember = "Value";
                cmbCourseFilter.SelectedIndex = 0;

                reader.Close();
            }
        }

        private void LoadStudents()
        {
            // Pure ADO.NET - SqlDataAdapter fills DataTable
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                                    s.Email, s.Phone, s.GPA, c.CourseName, 
                                    CASE WHEN s.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
                                    FROM Students s
                                    INNER JOIN Courses c ON s.CourseId = c.CourseId";

                    // Apply course filter
                    if (cmbCourseFilter != null && cmbCourseFilter.SelectedItem != null)
                    {
                        var selectedCourse = cmbCourseFilter.SelectedItem as ComboBoxItem;
                        if (selectedCourse != null && selectedCourse.Value > 0)
                        {
                            query += $" WHERE s.CourseId = {selectedCourse.Value}";
                        }
                    }

                    query += " ORDER BY s.StudentId DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvStudents.DataSource = dt;

                    // Style columns
                    if (dgvStudents.Columns.Count > 0)
                    {
                        dgvStudents.Columns["StudentId"].HeaderText = "ID";
                        dgvStudents.Columns["StudentId"].Width = 50;
                        dgvStudents.Columns["StudentNumber"].HeaderText = "Student #";
                        dgvStudents.Columns["FirstName"].HeaderText = "First Name";
                        dgvStudents.Columns["LastName"].HeaderText = "Last Name";
                        dgvStudents.Columns["Email"].HeaderText = "Email";
                        dgvStudents.Columns["Phone"].HeaderText = "Phone";
                        dgvStudents.Columns["GPA"].HeaderText = "GPA";
                        dgvStudents.Columns["CourseName"].HeaderText = "Course";
                        dgvStudents.Columns["Status"].HeaderText = "Status";
                        dgvStudents.Columns["Status"].Width = 80;

                        // Add Edit and Delete buttons if not already added
                        if (!dgvStudents.Columns.Contains("EditButton"))
                        {
                            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                            {
                                Name = "EditButton",
                                HeaderText = "Edit",
                                Text = "Edit",
                                UseColumnTextForButtonValue = true,
                                Width = 70
                            };
                            dgvStudents.Columns.Add(btnEdit);
                        }

                        if (!dgvStudents.Columns.Contains("DeleteButton"))
                        {
                            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                            {
                                Name = "DeleteButton",
                                HeaderText = "Delete",
                                Text = "Delete",
                                UseColumnTextForButtonValue = true,
                                Width = 70
                            };
                            dgvStudents.Columns.Add(btnDelete);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadStudents();
                return;
            }

            // Direct ADO.NET search
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                                    s.Email, s.Phone, s.GPA, c.CourseName,
                                    CASE WHEN s.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
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

                    dgvStudents.DataSource = dt;

                    MessageBox.Show($"Found {dt.Rows.Count} student(s) matching '{keyword}'", "Search Results",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbCourseFilter.SelectedIndex = 0;
            LoadStudents();
        }

        private void CmbCourseFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            StudentAddForm addForm = new StudentAddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void DgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int studentId = Convert.ToInt32(dgvStudents.Rows[e.RowIndex].Cells["StudentId"].Value);

            // Edit button
            if (dgvStudents.Columns[e.ColumnIndex].Name == "EditButton")
            {
                StudentEditForm editForm = new StudentEditForm(studentId);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadStudents();
                }
            }
            // Delete button
            else if (dgvStudents.Columns[e.ColumnIndex].Name == "DeleteButton")
            {
                var result = MessageBox.Show("Are you sure you want to delete this student?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteStudent(studentId);
                }
            }
        }

        private void DeleteStudent(int studentId)
        {
            // Pure ADO.NET DELETE
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Students WHERE StudentId = @StudentId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadStudents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper class for ComboBox
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
    }
}
