<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="StoredProcedure_CRUD.EmployeeList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee List - CRUD with Stored Procedures</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="EmployeeList.aspx">
                    <i class="fas fa-users me-2"></i>Employee Management System
                </a>
                <span class="navbar-text text-white">
                    <i class="fas fa-database me-1"></i>Stored Procedures Demo
                </span>
            </div>
        </nav>

        <div class="container mt-4">
            <!-- Page Header -->
            <div class="row mb-4">
                <div class="col-12">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h2>
                                <i class="fas fa-list me-2 text-primary"></i>Employee List
                            </h2>
                            <p class="text-muted">Manage employees using ADO.NET with Stored Procedures</p>
                        </div>
                        <div>
                            <a href="EmployeeAdd.aspx" class="btn btn-success btn-lg">
                                <i class="fas fa-plus me-2"></i>Add New Employee
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Alert Messages -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                <asp:Label ID="lblMessage" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </asp:Panel>

            <!-- Statistics Cards -->
            <div class="row mb-4">
                <div class="col-md-3 mb-3">
                    <div class="card stat-card bg-primary text-white shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-users fa-2x mb-2"></i>
                            <h3><asp:Label ID="lblTotalEmployees" runat="server" Text="0" /></h3>
                            <p class="mb-0">Total Employees</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card stat-card bg-success text-white shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-user-check fa-2x mb-2"></i>
                            <h3><asp:Label ID="lblActiveEmployees" runat="server" Text="0" /></h3>
                            <p class="mb-0">Active Employees</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card stat-card bg-info text-white shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-building fa-2x mb-2"></i>
                            <h3><asp:Label ID="lblTotalDepartments" runat="server" Text="0" /></h3>
                            <p class="mb-0">Departments</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card stat-card bg-warning text-white shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-dollar-sign fa-2x mb-2"></i>
                            <h3><asp:Label ID="lblAverageSalary" runat="server" Text="$0" /></h3>
                            <p class="mb-0">Avg Salary</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Search and Filter -->
            <div class="row mb-3">
                <div class="col-md-8">
                    <div class="input-group">
                        <span class="input-group-text bg-primary text-white">
                            <i class="fas fa-search"></i>
                        </span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, email, department, or position..." />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" />
                    </div>
                </div>
                <div class="col-md-4 text-end">
                    <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                        <asp:ListItem Value="All" Selected="True">All Employees</asp:ListItem>
                        <asp:ListItem Value="Active">Active Only</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive Only</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- Employee Table -->
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">
                        <i class="fas fa-table me-2"></i>Employee Records
                        <span class="badge bg-light text-primary float-end">
                            <asp:Label ID="lblRecordCount" runat="server" />
                        </span>
                    </h5>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvEmployees" runat="server" CssClass="table table-hover table-striped mb-0"
                            AutoGenerateColumns="False" DataKeyNames="EmployeeId"
                            OnRowCommand="gvEmployees_RowCommand"
                            EmptyDataText="No employees found. Click 'Add New Employee' to create one.">
                            <Columns>
                                <asp:BoundField DataField="EmployeeId" HeaderText="ID" ItemStyle-CssClass="text-center" />
                                
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <div class="d-flex align-items-center">
                                            <div class="avatar-circle bg-primary text-white me-2">
                                                <%# Eval("FirstName").ToString().Substring(0,1) + Eval("LastName").ToString().Substring(0,1) %>
                                            </div>
                                            <div>
                                                <strong><%# Eval("FullName") %></strong>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Contact">
                                    <ItemTemplate>
                                        <div>
                                            <i class="fas fa-envelope text-primary me-1"></i><%# Eval("Email") %><br />
                                            <i class="fas fa-phone text-success me-1"></i><%# Eval("Phone") ?? "N/A" %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <span class="badge bg-info"><%# Eval("Department") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="Position" HeaderText="Position" />
                                
                                <asp:TemplateField HeaderText="Salary">
                                    <ItemTemplate>
                                        <strong class="text-success"><%# Eval("FormattedSalary") %></strong>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Hire Date">
                                    <ItemTemplate>
                                        <%# Eval("FormattedHireDate") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <div class="btn-group" role="group">
                                            <a href='<%# "EmployeeDetails.aspx?id=" + Eval("EmployeeId") %>' class="btn btn-sm btn-info" title="View Details">
                                                <i class="fas fa-eye"></i>
                                            </a>
                                            <a href='<%# "EmployeeEdit.aspx?id=" + Eval("EmployeeId") %>' class="btn btn-sm btn-warning" title="Edit">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" 
                                                CommandName="DeleteEmployee" CommandArgument='<%# Eval("EmployeeId") %>'
                                                OnClientClick="return confirm('Are you sure you want to delete this employee?');" title="Delete">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Info Box -->
            <div class="alert alert-info mt-4">
                <h5><i class="fas fa-info-circle me-2"></i>About This Application</h5>
                <p class="mb-0">
                    This application demonstrates CRUD operations using <strong>ADO.NET with Stored Procedures</strong>.
                    All database operations are performed through stored procedures like <code>sp_GetAllEmployees</code>,
                    <code>sp_InsertEmployee</code>, <code>sp_UpdateEmployee</code>, and <code>sp_DeleteEmployee</code>.
                </p>
            </div>
        </div>

        <!-- Footer -->
        <footer class="bg-dark text-white text-center py-3 mt-5">
            <p class="mb-0">
                &copy; <%= DateTime.Now.Year %> Employee Management System | 
                <i class="fas fa-database me-1"></i>Powered by ADO.NET Stored Procedures
            </p>
        </footer>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
