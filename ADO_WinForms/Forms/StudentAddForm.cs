using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ADO_WinForms.Forms
{
    public partial class StudentAddForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;
        private TextBox txtStudentNumber = null!, txtFirstName = null!, txtLastName = null!, txtEmail = null!, txtPhone = null!, txtGPA = null!;
        private DateTimePicker dtpDateOfBirth = null!;
        private ComboBox cmbCourse = null!;
        private CheckBox chkIsActive = null!;

        public StudentAddForm()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.Text = "Add New Student";
            this.Size = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title Panel
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(39, 174, 96)
            };

            Label titleLabel = new Label
            {
                Text = "Add New Student",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            titlePanel.Controls.Add(titleLabel);

            // Form Panel
            Panel formPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(540, 450),
                BackColor = Color.White
            };

            int y = 20;

            // Student Number
            AddFormField(formPanel, "Student Number:", ref txtStudentNumber, ref y);

            // First Name
            AddFormField(formPanel, "First Name:", ref txtFirstName, ref y);

            // Last Name
            AddFormField(formPanel, "Last Name:", ref txtLastName, ref y);

            // Email
            AddFormField(formPanel, "Email:", ref txtEmail, ref y);

            // Phone
            AddFormField(formPanel, "Phone:", ref txtPhone, ref y);

            // Date of Birth
            Label lblDOB = new Label
            {
                Text = "Date of Birth:",
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };

            dtpDateOfBirth = new DateTimePicker
            {
                Location = new Point(180, y),
                Size = new Size(340, 25),
                Format = DateTimePickerFormat.Short
            };
            dtpDateOfBirth.Value = new DateTime(2002, 1, 1);

            formPanel.Controls.AddRange(new Control[] { lblDOB, dtpDateOfBirth });
            y += 40;

            // Course
            Label lblCourse = new Label
            {
                Text = "Course:",
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };

            cmbCourse = new ComboBox
            {
                Location = new Point(180, y),
                Size = new Size(340, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            formPanel.Controls.AddRange(new Control[] { lblCourse, cmbCourse });
            y += 40;

            // GPA
            AddFormField(formPanel, "GPA:", ref txtGPA, ref y);
            txtGPA.Text = "0.00";

            // Is Active
            chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(180, y),
                Size = new Size(100, 25),
                Checked = true
            };

            formPanel.Controls.Add(chkIsActive);

            // Buttons
            Button btnSave = new Button
            {
                Text = "Save Student",
                Location = new Point(300, 560),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(440, 560),
                Size = new Size(120, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titlePanel, formPanel, btnSave, btnCancel });
        }

        private void AddFormField(Panel panel, string labelText, ref TextBox textBox, ref int y)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };

            textBox = new TextBox
            {
                Location = new Point(180, y),
                Size = new Size(340, 25)
            };

            panel.Controls.AddRange(new Control[] { label, textBox });
            y += 40;
        }

        private void LoadCourses()
        {
            // Pure ADO.NET - Load courses
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand("SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName", conn);

                conn.Open();
                reader = cmd.ExecuteReader();

                cmbCourse.Items.Clear();

                while (reader.Read())
                {
                    cmbCourse.Items.Add(new ComboBoxItem
                    {
                        Text = reader["CourseName"].ToString(),
                        Value = Convert.ToInt32(reader["CourseId"])
                    });
                }

                cmbCourse.DisplayMember = "Text";
                cmbCourse.ValueMember = "Value";

                if (cmbCourse.Items.Count > 0)
                    cmbCourse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtStudentNumber.Text))
            {
                MessageBox.Show("Please enter student number", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentNumber.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Please enter first name", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Please enter last name", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return;
            }

            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a course", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pure ADO.NET INSERT
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(connectionString);

                string query = @"INSERT INTO Students 
                                (StudentNumber, FirstName, LastName, Email, Phone, DateOfBirth, CourseId, GPA, IsActive, EnrollmentDate, CreatedDate)
                                VALUES 
                                (@StudentNumber, @FirstName, @LastName, @Email, @Phone, @DateOfBirth, @CourseId, @GPA, @IsActive, GETDATE(), GETDATE());
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value.Date);
                cmd.Parameters.AddWithValue("@CourseId", ((ComboBoxItem)cmbCourse.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@GPA", string.IsNullOrWhiteSpace(txtGPA.Text) ? 0 : Convert.ToDecimal(txtGPA.Text));
                cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                conn.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                MessageBox.Show($"Student added successfully! (ID: {newId})", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Unique constraint
                {
                    MessageBox.Show("Student number already exists!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database error: {sqlEx.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
    }
}
