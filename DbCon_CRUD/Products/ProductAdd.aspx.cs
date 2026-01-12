using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using DbCon_CRUD.Utilities;

namespace DbCon_CRUD.Products
{
    public partial class ProductAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // Using DbCon.ExecuteScalar to INSERT and get new ID
                string query = @"INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, IsActive, CreatedDate)
                                VALUES (@ProductName, @CategoryId, @Price, @StockQuantity, @Description, @IsActive, GETDATE());
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                SqlParameter[] parameters = {
                    DbCon.CreateParameter("@ProductName", txtProductName.Text.Trim()),
                    DbCon.CreateParameter("@CategoryId", Convert.ToInt32(ddlCategory.SelectedValue)),
                    DbCon.CreateParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
                    DbCon.CreateParameter("@StockQuantity", Convert.ToInt32(txtStock.Text)),
                    DbCon.CreateParameter("@Description", string.IsNullOrWhiteSpace(txtDescription.Text) ? (object)DBNull.Value : txtDescription.Text.Trim()),
                    DbCon.CreateParameter("@IsActive", chkIsActive.Checked)
                };

                int newProductId = Convert.ToInt32(DbCon.ExecuteScalar(query, parameters));

                Response.Redirect($"ProductList.aspx?success=Product added successfully! (ID: {newProductId})");
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
