using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using DbCon_CRUD.Utilities;

namespace DbCon_CRUD.Products
{
    public partial class ProductEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProductData();
            }
        }

        private void LoadCategories()
        {
            // Using DbCon.ExecuteDataTable
            string query = "SELECT CategoryId, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
            DataTable dt = DbCon.ExecuteDataTable(query);

            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("-- Select Category --", "0"));

            foreach (DataRow row in dt.Rows)
            {
                ddlCategory.Items.Add(new ListItem(row["CategoryName"].ToString(), row["CategoryId"].ToString()));
            }
        }

        private void LoadProductData()
        {
            int productId = 0;
            if (Request.QueryString["id"] != null)
            {
                productId = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                Response.Redirect("ProductList.aspx");
                return;
            }

            // Using DbCon.ExecuteReader to load single product
            string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
            SqlParameter[] parameters = {
                DbCon.CreateParameter("@ProductId", productId)
            };

            using (SqlDataReader reader = DbCon.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    txtProductId.Text = reader["ProductId"].ToString();
                    txtProductName.Text = reader["ProductName"].ToString();
                    ddlCategory.SelectedValue = reader["CategoryId"].ToString();
                    txtPrice.Text = reader["Price"].ToString();
                    txtStock.Text = reader["StockQuantity"].ToString();
                    txtDescription.Text = reader["Description"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);
                }
                else
                {
                    Response.Redirect("ProductList.aspx");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Using DbCon.ExecuteNonQuery for UPDATE
                string query = @"UPDATE Products 
                                SET ProductName = @ProductName,
                                    CategoryId = @CategoryId,
                                    Price = @Price,
                                    StockQuantity = @StockQuantity,
                                    Description = @Description,
                                    IsActive = @IsActive,
                                    ModifiedDate = GETDATE()
                                WHERE ProductId = @ProductId";

                SqlParameter[] parameters = {
                    DbCon.CreateParameter("@ProductId", Convert.ToInt32(txtProductId.Text)),
                    DbCon.CreateParameter("@ProductName", txtProductName.Text.Trim()),
                    DbCon.CreateParameter("@CategoryId", Convert.ToInt32(ddlCategory.SelectedValue)),
                    DbCon.CreateParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
                    DbCon.CreateParameter("@StockQuantity", Convert.ToInt32(txtStock.Text)),
                    DbCon.CreateParameter("@Description", string.IsNullOrWhiteSpace(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text.Trim()),
                    DbCon.CreateParameter("@IsActive", chkIsActive.Checked)
                };

                int rowsAffected = DbCon.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    Response.Redirect("ProductList.aspx?success=Product updated successfully!");
                }
                else
                {
                    ShowMessage("Failed to update product", "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.CssClass = $"alert alert-{type}";
            pnlMessage.Controls.Clear();
            pnlMessage.Controls.Add(new System.Web.UI.LiteralControl(message));
            pnlMessage.Visible = true;
        }
    }
}
