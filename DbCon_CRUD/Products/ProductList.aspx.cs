using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using DbCon_CRUD.Utilities;

namespace DbCon_CRUD.Products
{
    public partial class ProductList : System.Web.UI.Page
    {
        // Direct use of DbCon class - no BLL or DAL layer
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadStatistics();
                LoadProducts();

                if (Request.QueryString["success"] != null)
                {
                    ShowMessage(Request.QueryString["success"], "success");
                }
            }
        }

        private void LoadCategories()
        {
            // Using DbCon.ExecuteDataTable to load categories
            string query = "SELECT CategoryId, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
            DataTable dt = DbCon.ExecuteDataTable(query);

            ddlCategoryFilter.Items.Clear();
            ddlCategoryFilter.Items.Add(new ListItem("All Categories", "0"));

            foreach (DataRow row in dt.Rows)
            {
                ddlCategoryFilter.Items.Add(new ListItem(row["CategoryName"].ToString(), row["CategoryId"].ToString()));
            }
        }

        private void LoadStatistics()
        {
            // Using DbCon.ExecuteDataTable to get statistics from view
            string query = "SELECT * FROM vw_ProductStatistics";
            DataTable dt = DbCon.ExecuteDataTable(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblTotalProducts.Text = row["TotalProducts"].ToString();
                lblTotalStock.Text = row["TotalStock"].ToString();
                lblAvgPrice.Text = Convert.ToDecimal(row["AveragePrice"]).ToString("C0");
                lblInventoryValue.Text = Convert.ToDecimal(row["TotalInventoryValue"]).ToString("C0");
            }
        }

        private void LoadProducts()
        {
            // Using DbCon.ExecuteDataTable with JOIN
            string query = @"SELECT p.ProductId, p.ProductName, c.CategoryName, p.Price, 
                            p.StockQuantity, p.Description, p.IsActive, p.CreatedDate,
                            (p.Price * p.StockQuantity) AS TotalValue,
                            FORMAT(p.Price, 'C') AS FormattedPrice,
                            CASE WHEN p.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
                            FROM Products p
                            INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                            WHERE 1=1 ";

            // Add category filter
            int categoryId = Convert.ToInt32(ddlCategoryFilter.SelectedValue);
            if (categoryId > 0)
            {
                query += " AND p.CategoryId = @CategoryId";
            }

            query += " ORDER BY p.ProductId DESC";

            SqlParameter[] parameters = categoryId > 0 
                ? new[] { DbCon.CreateParameter("@CategoryId", categoryId) } 
                : null;

            DataTable dt = DbCon.ExecuteDataTable(query, parameters);
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadProducts();
                return;
            }

            // Using DbCon.ExecuteDataTable with LIKE search
            string query = @"SELECT p.ProductId, p.ProductName, c.CategoryName, p.Price, 
                            p.StockQuantity, p.Description, p.IsActive,
                            (p.Price * p.StockQuantity) AS TotalValue,
                            FORMAT(p.Price, 'C') AS FormattedPrice,
                            CASE WHEN p.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
                            FROM Products p
                            INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                            WHERE p.ProductName LIKE @Keyword 
                               OR p.Description LIKE @Keyword
                               OR c.CategoryName LIKE @Keyword
                            ORDER BY p.ProductId DESC";

            SqlParameter[] parameters = {
                DbCon.CreateParameter("@Keyword", "%" + keyword + "%")
            };

            DataTable dt = DbCon.ExecuteDataTable(query, parameters);
            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            ShowMessage($"Found {dt.Rows.Count} product(s) matching '{keyword}'", "info");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlCategoryFilter.SelectedIndex = 0;
            LoadProducts();
            LoadStatistics();
            pnlMessage.Visible = false;
        }

        protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                try
                {
                    int productId = Convert.ToInt32(e.CommandArgument);

                    // Using DbCon.ExecuteNonQuery to delete
                    string query = "DELETE FROM Products WHERE ProductId = @ProductId";
                    SqlParameter[] parameters = {
                        DbCon.CreateParameter("@ProductId", productId)
                    };

                    int rowsAffected = DbCon.ExecuteNonQuery(query, parameters);

                    if (rowsAffected > 0)
                    {
                        ShowMessage("Product deleted successfully!", "success");
                        LoadProducts();
                        LoadStatistics();
                    }
                    else
                    {
                        ShowMessage("Failed to delete product", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error: " + ex.Message, "danger");
                }
            }
        }

        protected string GetStockBadgeClass(int stock)
        {
            if (stock == 0) return "bg-danger";
            if (stock < 10) return "bg-warning";
            return "bg-success";
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlMessage.Visible = true;
        }
    }
}
