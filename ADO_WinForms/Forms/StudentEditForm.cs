using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ADO_WinForms.Forms
{
    public partial class StudentEditForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;
        private int studentId;
        private TextBox txtStudentId, txtStudentNumber, txtFirstName, txtLastName, txtEmail, txtPhone, txtGPA;
        private DateTimePicker dtpDateOfBirth;
        private ComboBox cmbCourse;
        private CheckBox chkIsActive;

        public StudentEditForm(int id)
        {
            this.studentId = id;
            InitializeComponent();
            LoadCourses();
            LoadStudentData();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Student";
            this.Size = new Size(600, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title Panel
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(230, 126, 34)
            };

            Label titleLabel = new Label
            {
                Text = "Edit Student",
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
                Size = new Size(540, 500),
                BackColor = Color.White
            };

            int y = 20;

            // Student ID (readonly)
            Label lblId = new Label
            {
                Text = "Student ID:",
                Location = new Point(20, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };

            txtStudentId = new TextBox
            {
                Location = new Point(180, y),
                Size = new Size(340, 25),
                ReadOnly = true,
                BackColor = Color.LightGray
            };

            formPanel.Controls.AddRange(new Control[] { lblId, txtStudentId });
            y += 40;

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

            // Is Active
            chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(180, y),
                Size = new Size(100, 25)
            };

            formPanel.Controls.Add(chkIsActive);

            // Buttons
            Button btnUpdate = new Button
            {
                Text = "Update Student",
                Location = new Point(300, 610),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnUpdate.Click += BtnUpdate_Click;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(440, 610),
                Size = new Size(120, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titlePanel, formPanel, btnUpdate, btnCancel });
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

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

                reader.Close();
            }
        }

        private void LoadStudentData()
        {
            // Pure ADO.NET - Load student data
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
                    txtEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                    txtPhone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                    dtpDateOfBirth.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                    txtGPA.Text = reader["GPA"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);

                    int courseId = Convert.ToInt32(reader["CourseId"]);

                    // Select course
                    foreach (ComboBoxItem item in cmbCourse.Items)
                    {
                        if (item.Value == courseId)
                        {
                            cmbCourse.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student: {ex.Message}", "Error",
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            // Pure ADO.NET UPDATE
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
                                    Phone = @Phone,
                                    DateOfBirth = @DateOfBirth,
                                    CourseId = @CourseId,
                                    GPA = @GPA,
                                    IsActive = @IsActive,
                                    ModifiedDate = GETDATE()
                                WHERE StudentId = @StudentId";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@StudentId", Convert.ToInt32(txtStudentId.Text));
                cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value.Date);
                cmd.Parameters.AddWithValue("@CourseId", ((ComboBoxItem)cmbCourse.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
                cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Student updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
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
