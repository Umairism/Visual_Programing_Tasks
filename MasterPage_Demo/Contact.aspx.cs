using System;

namespace MasterPage_Demo
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Get form values
                    string name = txtName.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string phone = txtPhone.Text.Trim();
                    string subject = ddlSubject.SelectedValue;
                    string message = txtMessage.Text.Trim();
                    bool newsletter = chkNewsletter.Checked;

                    // In a real application, you would:
                    // 1. Save to database
                    // 2. Send email notification
                    // 3. Add to newsletter if subscribed

                    // Simulate processing
                    System.Threading.Thread.Sleep(500);

                    // Show success message
                    ShowMessage($"Thank you, {name}! Your message has been received. We'll respond to {email} within 24 hours.", "success");

                    // Clear form
                    ClearForm();

                    // Access master page to show notification
                    if (Master is Site masterPage)
                    {
                        masterPage.ShowMessage("Contact form submitted successfully!", "success");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"An error occurred: {ex.Message}", "danger");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearForm();
            ShowMessage("Form has been reset.", "info");
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            ddlSubject.SelectedIndex = 0;
            txtMessage.Text = string.Empty;
            chkNewsletter.Checked = false;
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            // Set CSS class based on type
            pnlMessage.CssClass = $"alert alert-{type} alert-dismissible fade show";
        }
    }
}
