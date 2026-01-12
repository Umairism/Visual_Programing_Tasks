using System;
using System.Web.UI;
using Taska.DataAccess;
using Taska.Models;

public partial class AddEdit : System.Web.UI.Page
{
    private StudentRepository repository = new StudentRepository();
    private int studentId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Check if we're editing an existing student
            if (Request.QueryString["id"] != null)
            {
                studentId = Convert.ToInt32(Request.QueryString["id"]);
                LoadStudentData(studentId);
                lblTitle.Text = "Edit Student";
            }
            else
            {
                // Set default enrollment date to today
                txtEnrollmentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                lblTitle.Text = "Add New Student";
            }
        }
    }

    private void LoadStudentData(int id)
    {
        Student student = repository.GetStudentById(id);
        
        if (student != null)
        {
            txtName.Text = student.Name;
            txtEmail.Text = student.Email;
            ddlCourse.SelectedValue = student.Course;
            txtEnrollmentDate.Text = student.EnrollmentDate.ToString("yyyy-MM-dd");
            txtPhone.Text = student.Phone;
        }
        else
        {
            ShowMessage("Student not found!", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Student student = new Student
                {
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Course = ddlCourse.SelectedValue,
                    EnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.Text),
                    Phone = txtPhone.Text.Trim()
                };

                bool success = false;

                // Check if we're editing or adding
                if (Request.QueryString["id"] != null)
                {
                    // UPDATE operation using LINQ
                    student.Id = Convert.ToInt32(Request.QueryString["id"]);
                    success = repository.UpdateStudent(student);
                }
                else
                {
                    // CREATE operation using LINQ
                    success = repository.AddStudent(student);
                }

                if (success)
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    ShowMessage("Error saving student. Please try again.", true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", true);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    private void ShowMessage(string message, bool visible)
    {
        lblMessage.Text = message;
        lblMessage.Visible = visible;
    }
}
