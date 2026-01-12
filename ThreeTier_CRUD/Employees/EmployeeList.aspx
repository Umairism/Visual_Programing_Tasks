<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="ThreeTier_CRUD.Employees.EmployeeList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee List - 3-Tier CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx">
                    <i class="fas fa-layer-group me-2"></i>3-Tier CRUD
                </a>
                <div class="navbar-text text-white">Employee Management</div>
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
                            <i class="fas fa-users fa-2x text-primary mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblTotalEmployees" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Total Employees</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-user-check fa-2x text-success mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblActiveEmployees" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Active</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-dollar-sign fa-2x text-info mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblAvgSalary" runat="server" Text="$0" /></h3>
                            <p class="text-muted mb-0">Avg Salary</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-building fa-2x text-warning mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblDepartmentCount" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Departments</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <div class="row align-items-center">
                        <div class="col">
                            <h4 class="mb-0"><i class="fas fa-users me-2"></i>Employee List</h4>
                            <small>Managed through Business Logic Layer (BLL)</small>
                        </div>
                        <div class="col-auto">
                            <a href="EmployeeAdd.aspx" class="btn btn-light btn-sm">
                                <i class="fas fa-plus me-2"></i>Add Employee
                            </a>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, email, position, or department" />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                            </div>
                        </div>
                        <div class="col-md-6 text-end">
                            <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-select d-inline-block w-auto" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                <asp:ListItem Value="All">All Employees</asp:ListItem>
                                <asp:ListItem Value="Active" Selected="True">Active Only</asp:ListItem>
                                <asp:ListItem Value="Inactive">Inactive Only</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvEmployees" runat="server" CssClass="table table-hover" AutoGenerateColumns="False" 
                            OnRowCommand="gvEmployees_RowCommand" DataKeyNames="EmployeeId">
                            <Columns>
                                <asp:TemplateField HeaderText="ID">
                                    <ItemTemplate>
                                        <span class="badge bg-secondary"><%# Eval("EmployeeId") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee">
                                    <ItemTemplate>
                                        <div class="d-flex align-items-center">
                                            <div class="avatar-circle bg-primary text-white me-2">
                                                <%# Eval("FirstName").ToString().Substring(0,1) + Eval("LastName").ToString().Substring(0,1) %>
                                            </div>
                                            <div>
                                                <strong><%# Eval("FullName") %></strong><br />
                                                <small class="text-muted"><%# Eval("Email") %></small>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                                <asp:BoundField DataField="Position" HeaderText="Position" />
                                <asp:BoundField DataField="FormattedSalary" HeaderText="Salary" />
                                <asp:BoundField DataField="FormattedHireDate" HeaderText="Hire Date" />
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
                                            <a href='<%# "EmployeeDetails.aspx?id=" + Eval("EmployeeId") %>' class="btn btn-info btn-sm" title="View">
                                                <i class="fas fa-eye"></i>
                                            </a>
                                            <a href='<%# "EmployeeEdit.aspx?id=" + Eval("EmployeeId") %>' class="btn btn-warning btn-sm" title="Edit">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                                CommandName="DeleteEmployee" CommandArgument='<%# Eval("EmployeeId") %>' 
                                                OnClientClick="return confirm('Are you sure you want to delete this employee?');" title="Delete">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center text-muted py-4">
                                    <i class="fas fa-inbox fa-3x mb-3"></i>
                                    <p>No employees found</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
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
