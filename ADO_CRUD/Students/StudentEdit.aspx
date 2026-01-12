<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentEdit.aspx.cs" Inherits="ADO_CRUD.Students.StudentEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Student</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-code me-2"></i>ADO.NET CRUD</a>
                <span class="navbar-text text-white">Edit Student</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="card shadow">
                        <div class="card-header bg-warning">
                            <h4 class="mb-0"><i class="fas fa-edit me-2"></i>Edit Student</h4>
                            <small>Using SqlCommand.ExecuteNonQuery() for UPDATE</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert"></asp:Panel>

                            <div class="mb-3">
                                <label class="form-label">Student ID</label>
                                <asp:TextBox ID="txtStudentId" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Student Number <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtStudentNumber" runat="server" CssClass="form-control" />
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
                                    <asp:TextBox ID="txtGPA" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
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
                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label">Active</label>
                                </div>
                            </div>

                            <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                <a href="StudentList.aspx" class="btn btn-secondary"><i class="fas fa-arrow-left me-2"></i>Cancel</a>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update Student" CssClass="btn btn-warning" OnClick="btnUpdate_Click" />
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
