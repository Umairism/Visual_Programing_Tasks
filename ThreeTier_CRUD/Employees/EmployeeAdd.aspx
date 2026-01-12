<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAdd.aspx.cs" Inherits="ThreeTier_CRUD.Employees.EmployeeAdd" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Employee - 3-Tier CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-layer-group me-2"></i>3-Tier CRUD</a>
                <span class="navbar-text text-white">Add Employee</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card shadow">
                        <div class="card-header bg-success text-white">
                            <h4 class="mb-0"><i class="fas fa-user-plus me-2"></i>Add New Employee</h4>
                            <small>All validation handled by Business Logic Layer (BLL)</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert"></asp:Panel>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" 
                                        ErrorMessage="First name is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Last Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" 
                                        ErrorMessage="Last name is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Email <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                    ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Phone</label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Department <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select" />
                                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment" 
                                        InitialValue="0" ErrorMessage="Department is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Position <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvPosition" runat="server" ControlToValidate="txtPosition" 
                                        ErrorMessage="Position is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Salary <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvSalary" runat="server" ControlToValidate="txtSalary" 
                                        ErrorMessage="Salary is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Hire Date <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtHireDate" runat="server" CssClass="form-control" TextMode="Date" />
                                    <asp:RequiredFieldValidator ID="rfvHireDate" runat="server" ControlToValidate="txtHireDate" 
                                        ErrorMessage="Hire date is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label">Active</label>
                                </div>
                            </div>

                            <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                <a href="EmployeeList.aspx" class="btn btn-secondary"><i class="fas fa-arrow-left me-2"></i>Cancel</a>
                                <asp:Button ID="btnSave" runat="server" Text="Save Employee" CssClass="btn btn-success" OnClick="btnSave_Click" />
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
