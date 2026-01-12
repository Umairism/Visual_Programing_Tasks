<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="DbCon_CRUD.Products.ProductEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Product</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-success">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-database me-2"></i>DbCon CRUD</a>
                <span class="navbar-text text-white">Edit Product</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card shadow">
                        <div class="card-header bg-warning">
                            <h4 class="mb-0"><i class="fas fa-edit me-2"></i>Edit Product</h4>
                            <small>Using DbCon.ExecuteNonQuery() for UPDATE</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert"></asp:Panel>

                            <div class="mb-3">
                                <label class="form-label">Product ID</label>
                                <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Product Name <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName" 
                                    ErrorMessage="Product name is required" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Category <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory" 
                                    InitialValue="0" ErrorMessage="Category is required" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Price <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                                    <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" 
                                        ErrorMessage="Price is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Stock Quantity <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvStock" runat="server" ControlToValidate="txtStock" 
                                        ErrorMessage="Stock quantity is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Description</label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>

                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label">Active</label>
                                </div>
                            </div>

                            <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                <a href="ProductList.aspx" class="btn btn-secondary"><i class="fas fa-arrow-left me-2"></i>Cancel</a>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update Product" CssClass="btn btn-warning" OnClick="btnUpdate_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
