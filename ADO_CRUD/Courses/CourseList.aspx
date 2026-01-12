<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseList.aspx.cs" Inherits="ADO_CRUD.Courses.CourseList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course List</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="../Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="../Default.aspx"><i class="fas fa-code me-2"></i>ADO.NET CRUD</a>
                <span class="navbar-text text-white">Course Management</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="card shadow">
                <div class="card-header bg-info text-white">
                    <h4 class="mb-0"><i class="fas fa-book me-2"></i>Course List</h4>
                    <small>Using SqlDataAdapter for data binding</small>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCourses" runat="server" CssClass="table table-hover" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="CourseId" HeaderText="ID" />
                                <asp:BoundField DataField="CourseCode" HeaderText="Code" />
                                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                <asp:BoundField DataField="Credits" HeaderText="Credits" />
                                <asp:BoundField DataField="Department" HeaderText="Department" />
                                <asp:BoundField DataField="StudentCount" HeaderText="Students Enrolled" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center text-muted py-4">
                                    <i class="fas fa-inbox fa-3x mb-3"></i>
                                    <p>No courses found</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer">
                    <small class="text-muted">
                        <i class="fas fa-code me-1"></i>
                        Pure ADO.NET: Direct SqlDataAdapter usage
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
