<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="DbCon_CRUD.Products.ProductList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product List - DbCon CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-success">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-database me-2"></i>DbCon CRUD</a>
                <span class="navbar-text text-white">Product Management</span>
            </div>
        </nav>

        <div class="container mt-4">
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
                <asp:Label ID="lblMessage" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </asp:Panel>

            <!-- Statistics Cards -->
            <div class="row mb-4">
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-box fa-2x text-success mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblTotalProducts" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Total Products</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-warehouse fa-2x text-info mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblTotalStock" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Total Stock</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-dollar-sign fa-2x text-warning mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblAvgPrice" runat="server" Text="$0" /></h3>
                            <p class="text-muted mb-0">Avg Price</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-chart-line fa-2x text-danger mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblInventoryValue" runat="server" Text="$0" /></h3>
                            <p class="text-muted mb-0">Inventory Value</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card shadow">
                <div class="card-header bg-success text-white">
                    <div class="row align-items-center">
                        <div class="col">
                            <h4 class="mb-0"><i class="fas fa-box me-2"></i>Product Inventory</h4>
                            <small>Using DbCon.ExecuteDataTable() method</small>
                        </div>
                        <div class="col-auto">
                            <a href="ProductAdd.aspx" class="btn btn-light btn-sm">
                                <i class="fas fa-plus me-2"></i>Add Product
                            </a>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name or description" />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                            </div>
                        </div>
                        <div class="col-md-6 text-end">
                            <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-select d-inline-block w-auto" 
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvProducts" runat="server" CssClass="table table-hover" AutoGenerateColumns="False" 
                            OnRowCommand="gvProducts_RowCommand" DataKeyNames="ProductId">
                            <Columns>
                                <asp:BoundField DataField="ProductId" HeaderText="ID" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                <asp:BoundField DataField="FormattedPrice" HeaderText="Price" />
                                <asp:TemplateField HeaderText="Stock">
                                    <ItemTemplate>
                                        <span class='badge <%# GetStockBadgeClass((int)Eval("StockQuantity")) %>'>
                                            <%# Eval("StockQuantity") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TotalValue" HeaderText="Total Value" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <a href='<%# "ProductEdit.aspx?id=" + Eval("ProductId") %>' class="btn btn-warning btn-sm" title="Edit">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                                CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductId") %>' 
                                                OnClientClick="return confirm('Are you sure you want to delete this product?');" title="Delete">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center text-muted py-4">
                                    <i class="fas fa-inbox fa-3x mb-3"></i>
                                    <p>No products found</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer">
                    <small class="text-muted">
                        <i class="fas fa-code me-1"></i>
                        Using: <code>DbCon.ExecuteDataTable("SELECT...")</code>
                    </small>
                </div>
            </div>

            <div class="mt-3 text-center">
                <a href="../Default.aspx" class="btn btn-secondary">
                    <i class="fas fa-arrow-left me-2"></i>Back to Home
                </a>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
