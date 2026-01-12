<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentAdd.aspx.cs" Inherits="ADO_CRUD.Students.StudentAdd" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Student</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-code me-2"></i>ADO.NET CRUD</a>
                <span class="navbar-text text-white">Add Student</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card shadow">
                        <div class="card-header bg-primary text-white">
                            <h4 class="mb-0"><i class="fas fa-user-plus me-2"></i>Add New Student</h4>
                            <small>Using SqlCommand.ExecuteScalar() for INSERT</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert"></asp:Panel>

                            <div class="mb-3">
                                <label class="form-label">Student Number <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtStudentNumber" runat="server" CssClass="form-control" placeholder="S2024XXX" />
                                <asp:RequiredFieldValidator ID="rfvStudentNumber" runat="server" ControlToValidate="txtStudentNumber" 
                                    ErrorMessage="Student number is required" CssClass="text-danger" Display="Dynamic" />
                            </div>

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
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Date of Birth <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" TextMode="Date" />
                                    <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtDateOfBirth" 
                                        ErrorMessage="Date of birth is required" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">GPA</label>
                                    <asp:TextBox ID="txtGPA" runat="server" CssClass="form-control" TextMode="Number" step="0.01" Text="0.00" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Course <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvCourse" runat="server" ControlToValidate="ddlCourse" 
                                    InitialValue="0" ErrorMessage="Course is required" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label">Active</label>
                                </div>
                            </div>

                            <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                <a href="StudentList.aspx" class="btn btn-secondary"><i class="fas fa-arrow-left me-2"></i>Cancel</a>
                                <asp:Button ID="btnSave" runat="server" Text="Save Student" CssClass="btn btn-primary" OnClick="btnSave_Click" />
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
