using System;

namespace MasterPage_Demo
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page
            }
        }
    }
}
