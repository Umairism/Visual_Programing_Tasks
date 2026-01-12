using System;

namespace MasterPage_Demo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
            }
        }

        private void LoadStatistics()
        {
            // Simulate loading statistics
            lblCustomers.Text = "1,250+";
            lblProjects.Text = "2,500+";
            lblAwards.Text = "50+";
            lblYears.Text = "6+";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Access master page and show a welcome message
            if (Master is Site masterPage)
            {
                // You can interact with master page here
                // For example: masterPage.ShowMessage("Welcome to TechCorp!", "success");
            }
        }
    }
}
