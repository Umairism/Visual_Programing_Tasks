<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentList.aspx.cs" Inherits="ThreeTier_CRUD.Departments.DepartmentList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Department List</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-layer-group me-2"></i>3-Tier CRUD</a>
                <span class="navbar-text text-white">Department Management</span>
            </div>
        </nav>
        <div class="container mt-4">
            <div class="card shadow">
                <div class="card-header bg-success text-white">
                    <h4>Departments - Managed through BLL</h4>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvDepartments" runat="server" CssClass="table table-hover" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="DepartmentId" HeaderText="ID" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="Name" />
                            <asp:BoundField DataField="DepartmentCode" HeaderText="Code" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
