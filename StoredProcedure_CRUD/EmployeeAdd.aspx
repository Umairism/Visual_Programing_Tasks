<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAdd.aspx.cs" Inherits="StoredProcedure_CRUD.EmployeeAdd" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Employee - Stored Procedures CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="EmployeeList.aspx">
                    <i class="fas fa-users me-2"></i>Employee Management
                </a>
                <span class="navbar-text text-white">Add New Employee</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row">
                <div class="col-md-8 offset-md-2">
                    <div class="card shadow">
                        <div class="card-header bg-success text-white">
                            <h4 class="mb-0">
                                <i class="fas fa-user-plus me-2"></i>Add New Employee
                            </h4>
                            <small>Insert employee using sp_InsertEmployee stored procedure</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
                                <asp:Label ID="lblMessage" runat="server" />
                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                            </asp:Panel>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-user me-1"></i>First Name *
                                    </label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Enter first name" />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                        ErrorMessage="First name is required" Display="Dynamic" CssClass="text-danger small" />
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-user me-1"></i>Last Name *
                                    </label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter last name" />
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                        ErrorMessage="Last name is required" Display="Dynamic" CssClass="text-danger small" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-envelope me-1"></i>Email *
                                    </label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="email@example.com" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email is required" Display="Dynamic" CssClass="text-danger small" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                                        ErrorMessage="Invalid email format" Display="Dynamic" CssClass="text-danger small" />
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-phone me-1"></i>Phone
                                    </label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="555-0123" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-building me-1"></i>Department *
                                    </label>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="">-- Select Department --</asp:ListItem>
                                        <asp:ListItem Value="IT">IT</asp:ListItem>
                                        <asp:ListItem Value="HR">HR</asp:ListItem>
                                        <asp:ListItem Value="Finance">Finance</asp:ListItem>
                                        <asp:ListItem Value="Marketing">Marketing</asp:ListItem>
                                        <asp:ListItem Value="Sales">Sales</asp:ListItem>
                                        <asp:ListItem Value="Operations">Operations</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment"
                                        InitialValue="" ErrorMessage="Department is required" Display="Dynamic" CssClass="text-danger small" />
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-briefcase me-1"></i>Position *
                                    </label>
                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" placeholder="Job title" />
                                    <asp:RequiredFieldValidator ID="rfvPosition" runat="server" ControlToValidate="txtPosition"
                                        ErrorMessage="Position is required" Display="Dynamic" CssClass="text-danger small" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-dollar-sign me-1"></i>Salary *
                                    </label>
                                    <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" TextMode="Number" 
                                        placeholder="50000" step="0.01" />
                                    <asp:RequiredFieldValidator ID="rfvSalary" runat="server" ControlToValidate="txtSalary"
                                        ErrorMessage="Salary is required" Display="Dynamic" CssClass="text-danger small" />
                                    <asp:RangeValidator ID="rvSalary" runat="server" ControlToValidate="txtSalary"
                                        MinimumValue="0" MaximumValue="999999999" Type="Double"
                                        ErrorMessage="Salary must be a valid amount" Display="Dynamic" CssClass="text-danger small" />
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">
                                        <i class="fas fa-calendar me-1"></i>Hire Date *
                                    </label>
                                    <asp:TextBox ID="txtHireDate" runat="server" CssClass="form-control" TextMode="Date" />
                                    <asp:RequiredFieldValidator ID="rfvHireDate" runat="server" ControlToValidate="txtHireDate"
                                        ErrorMessage="Hire date is required" Display="Dynamic" CssClass="text-danger small" />
                                </div>
                            </div>

                            <div class="alert alert-info">
                                <i class="fas fa-info-circle me-2"></i>
                                <strong>Note:</strong> This form uses <code>sp_InsertEmployee</code> stored procedure with output parameter to insert data.
                            </div>

                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                <a href="EmployeeList.aspx" class="btn btn-secondary">
                                    <i class="fas fa-times me-2"></i>Cancel
                                </a>
                                <asp:Button ID="btnSave" runat="server" Text="Save Employee" CssClass="btn btn-success"
                                    OnClick="btnSave_Click" />
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
