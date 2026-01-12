using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace MasterPage_Demo
{
    public partial class Products : System.Web.UI.Page
    {
        private static List<Product> allProducts;
        private string currentFilter = "All";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeProducts();
                LoadProducts("All");
            }
            else
            {
                // Restore filter state
                if (ViewState["CurrentFilter"] != null)
                {
                    currentFilter = ViewState["CurrentFilter"].ToString();
                }
            }
        }

        private void InitializeProducts()
        {
            if (allProducts == null)
            {
                allProducts = new List<Product>
                {
                    new Product
                    {
                        ProductId = 1,
                        Name = "CloudHub Pro",
                        Category = "Cloud",
                        Description = "Enterprise cloud hosting solution with unlimited scalability and 99.9% uptime guarantee.",
                        Features = "Auto-scaling,Load balancing,24/7 monitoring,Data backup",
                        Price = "$299/month",
                        Popularity = "Best Seller"
                    },
                    new Product
                    {
                        ProductId = 2,
                        Name = "DataMaster Suite",
                        Category = "Software",
                        Description = "Complete data analytics platform with AI-powered insights and real-time dashboards.",
                        Features = "Real-time analytics,AI insights,Custom reports,API integration",
                        Price = "$199/month",
                        Popularity = "Popular"
                    },
                    new Product
                    {
                        ProductId = 3,
                        Name = "SecureShield Plus",
                        Category = "Security",
                        Description = "Advanced cybersecurity solution protecting against modern threats and vulnerabilities.",
                        Features = "Threat detection,Firewall,VPN,Malware protection",
                        Price = "$149/month",
                        Popularity = "Trending"
                    },
                    new Product
                    {
                        ProductId = 4,
                        Name = "DevOps Accelerator",
                        Category = "Software",
                        Description = "Streamline your development workflow with automated CI/CD pipelines and deployment tools.",
                        Features = "CI/CD automation,Container support,Git integration,Testing tools",
                        Price = "$249/month",
                        Popularity = "New"
                    },
                    new Product
                    {
                        ProductId = 5,
                        Name = "CloudStorage Infinity",
                        Category = "Cloud",
                        Description = "Unlimited cloud storage with enterprise-grade security and global CDN delivery.",
                        Features = "Unlimited storage,CDN delivery,File sharing,Version control",
                        Price = "$99/month",
                        Popularity = "Popular"
                    },
                    new Product
                    {
                        ProductId = 6,
                        Name = "IdentityGuard",
                        Category = "Security",
                        Description = "Multi-factor authentication and identity management platform for enterprise security.",
                        Features = "MFA,SSO,User management,Audit logs",
                        Price = "$179/month",
                        Popularity = "Best Seller"
                    }
                };
            }
        }

        private void LoadProducts(string filter)
        {
            List<Product> filteredProducts;

            if (filter == "All")
            {
                filteredProducts = allProducts;
            }
            else
            {
                filteredProducts = allProducts.Where(p => p.Category == filter).ToList();
            }

            if (filteredProducts.Count > 0)
            {
                rptProducts.DataSource = filteredProducts;
                rptProducts.DataBind();
                pnlNoProducts.Visible = false;
            }
            else
            {
                rptProducts.DataSource = null;
                rptProducts.DataBind();
                pnlNoProducts.Visible = true;
            }

            // Store current filter
            ViewState["CurrentFilter"] = filter;
            currentFilter = filter;

            // Update button styles
            UpdateFilterButtons(filter);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string filter = btn.CommandArgument;
            LoadProducts(filter);
        }

        private void UpdateFilterButtons(string activeFilter)
        {
            btnAll.CssClass = activeFilter == "All" ? "btn btn-primary" : "btn btn-outline-primary";
            btnCloud.CssClass = activeFilter == "Cloud" ? "btn btn-primary" : "btn btn-outline-primary";
            btnSoftware.CssClass = activeFilter == "Software" ? "btn btn-primary" : "btn btn-outline-primary";
            btnSecurity.CssClass = activeFilter == "Security" ? "btn btn-primary" : "btn btn-outline-primary";
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                ShowProductDetails(productId);
            }
        }

        private void ShowProductDetails(int productId)
        {
            Product product = allProducts.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                lblProductName.Text = product.Name;
                lblProductCategory.Text = product.Category;
                lblProductDescription.Text = product.Description + " Features include: " + product.Features;
                lblProductPrice.Text = product.Price;
                pnlProductDetails.Visible = true;
            }
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            pnlProductDetails.Visible = false;
        }

        // Helper methods for Repeater
        protected string GetCategoryColor(string category)
        {
            switch (category)
            {
                case "Cloud": return "bg-primary";
                case "Software": return "bg-success";
                case "Security": return "bg-danger";
                default: return "bg-secondary";
            }
        }

        protected string GetCategoryIcon(string category)
        {
            switch (category)
            {
                case "Cloud": return "fas fa-cloud";
                case "Software": return "fas fa-laptop-code";
                case "Security": return "fas fa-shield-alt";
                default: return "fas fa-box";
            }
        }

        protected string GetFeaturesList(string features)
        {
            string[] featureArray = features.Split(',');
            string html = "";
            foreach (string feature in featureArray)
            {
                html += $"<li><i class='fas fa-check text-success me-2'></i>{feature.Trim()}</li>";
            }
            return html;
        }

        protected string GetPopularityBadge(string popularity)
        {
            switch (popularity)
            {
                case "Best Seller": return "bg-warning text-dark";
                case "Popular": return "bg-info text-white";
                case "Trending": return "bg-success text-white";
                case "New": return "bg-primary text-white";
                default: return "bg-secondary text-white";
            }
        }

        // Product class
        public class Product
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public string Features { get; set; }
            public string Price { get; set; }
            public string Popularity { get; set; }
        }
    }
}
