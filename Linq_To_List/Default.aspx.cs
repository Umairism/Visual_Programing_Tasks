using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Taska.DataAccess;

public partial class Default : System.Web.UI.Page
{
    private StudentRepository repository = new StudentRepository();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
            ShowMessage("", false);
        }
    }

    private void BindGrid()
    {
        gvStudents.DataSource = repository.GetAllStudents();
        gvStudents.DataBind();
        UpdateStudentCount();
    }

    private void UpdateStudentCount()
    {
        int count = repository.GetStudentCount();
        lblCount.Text = $"Total Students: {count}";
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddEdit.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string searchTerm = txtSearch.Text.Trim();
        var results = repository.SearchStudents(searchTerm);
        gvStudents.DataSource = results;
        gvStudents.DataBind();
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            ShowMessage($"Found {results.Count} student(s) matching '{searchTerm}'", true);
        }
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        BindGrid();
        ShowMessage("", false);
    }

    protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int studentId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditStudent")
        {
            Response.Redirect($"AddEdit.aspx?id={studentId}");
        }
        else if (e.CommandName == "DeleteStudent")
        {
            bool success = repository.DeleteStudent(studentId);
            
            if (success)
            {
                ShowMessage("Student deleted successfully!", true);
                BindGrid();
            }
            else
            {
                ShowMessage("Error deleting student.", true);
            }
        }
    }

    private void ShowMessage(string message, bool visible)
    {
        lblMessage.Text = message;
        lblMessage.Visible = visible;
    }
}
