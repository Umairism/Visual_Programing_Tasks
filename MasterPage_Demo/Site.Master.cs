using System;
using System.Web.UI;

namespace MasterPage_Demo
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set current time in footer
                lblCurrentTime.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
                
                // Highlight active menu item based on current page
                HighlightActiveMenuItem();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Search functionality
            string searchTerm = txtSearch.Text.Trim();
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Store search term in session
                Session["SearchTerm"] = searchTerm;
                
                // Show search results (you can redirect to a search results page)
                Response.Write($"<script>alert('Searching for: {searchTerm}');</script>");
                
                // Clear search box
                txtSearch.Text = string.Empty;
            }
        }

        private void HighlightActiveMenuItem()
        {
            // Get current page name
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            
            // Reset all menu items
            lnkHome.CssClass = "nav-link";
            lnkAbout.CssClass = "nav-link";
            lnkProducts.CssClass = "nav-link";
            lnkServices.CssClass = "nav-link";
            lnkContact.CssClass = "nav-link";
            
            // Highlight active menu item
            switch (currentPage.ToLower())
            {
                case "default.aspx":
                    lnkHome.CssClass = "nav-link active";
                    break;
                case "about.aspx":
                    lnkAbout.CssClass = "nav-link active";
                    break;
                case "products.aspx":
                    lnkProducts.CssClass = "nav-link active";
                    break;
                case "services.aspx":
                    lnkServices.CssClass = "nav-link active";
                    break;
                case "contact.aspx":
                    lnkContact.CssClass = "nav-link active";
                    break;
            }
        }

        // Public method that child pages can call
        public void ShowMessage(string message, string messageType = "info")
        {
            string icon = messageType == "success" ? "check-circle" : 
                         messageType == "warning" ? "exclamation-triangle" : 
                         messageType == "error" ? "times-circle" : "info-circle";
            
            string alertClass = messageType == "success" ? "alert-success" : 
                               messageType == "warning" ? "alert-warning" : 
                               messageType == "error" ? "alert-danger" : "alert-info";
            
            string script = $@"
                <script>
                    document.addEventListener('DOMContentLoaded', function() {{
                        var alertDiv = document.createElement('div');
                        alertDiv.className = 'alert {alertClass} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x mt-3';
                        alertDiv.style.zIndex = '9999';
                        alertDiv.innerHTML = '<i class=""fas fa-{icon} me-2""></i>{message}<button type=""button"" class=""btn-close"" data-bs-dismiss=""alert""></button>';
                        document.body.appendChild(alertDiv);
                        setTimeout(function() {{ alertDiv.remove(); }}, 5000);
                    }});
                </script>
            ";
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, false);
        }
    }
}
