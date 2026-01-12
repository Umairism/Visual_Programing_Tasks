<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="ADO_CRUD.Students.StudentList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student List - ADO.NET CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-code me-2"></i>ADO.NET CRUD</a>
                <span class="navbar-text text-white">Student Management</span>
            </div>
        </nav>

        <div class="container mt-4">
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
                <asp:Label ID="lblMessage" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </asp:Panel>

            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <div class="row align-items-center">
                        <div class="col">
                            <h4 class="mb-0"><i class="fas fa-user-graduate me-2"></i>Student List</h4>
                            <small>Using SqlDataAdapter and DataTable</small>
                        </div>
                        <div class="col-auto">
                            <a href="StudentAdd.aspx" class="btn btn-light btn-sm">
                                <i class="fas fa-plus me-2"></i>Add Student
                            </a>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, email, or student number" />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                            </div>
                        </div>
                        <div class="col-md-6 text-end">
                            <asp:DropDownList ID="ddlCourseFilter" runat="server" CssClass="form-select d-inline-block w-auto" 
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCourseFilter_SelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvStudents" runat="server" CssClass="table table-hover" AutoGenerateColumns="False" 
                            OnRowCommand="gvStudents_RowCommand" DataKeyNames="StudentId">
                            <Columns>
                                <asp:BoundField DataField="StudentId" HeaderText="ID" />
                                <asp:BoundField DataField="StudentNumber" HeaderText="Student #" />
                                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="CourseName" HeaderText="Course" />
                                <asp:TemplateField HeaderText="GPA">
                                    <ItemTemplate>
                                        <span class='<%# GetGPABadgeClass((decimal)Eval("GPA")) %>'>
                                            <%# Eval("GPA") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-sm">
                                            <a href='<%# "StudentEdit.aspx?id=" + Eval("StudentId") %>' class="btn btn-warning btn-sm" title="Edit">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                                CommandName="DeleteStudent" CommandArgument='<%# Eval("StudentId") %>' 
                                                OnClientClick="return confirm('Are you sure you want to delete this student?');" title="Delete">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center text-muted py-4">
                                    <i class="fas fa-inbox fa-3x mb-3"></i>
                                    <p>No students found</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer">
                    <small class="text-muted">
                        <i class="fas fa-code me-1"></i>
                        Using: <code>SqlDataAdapter.Fill(DataTable)</code> for GridView binding
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
