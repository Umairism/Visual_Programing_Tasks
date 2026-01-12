<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Management System - LINQ to List CRUD</title>
    <link href="Styles/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h1>Student Management System</h1>
                <p>ASP.NET Web Forms with LINQ to List</p>
            </header>

            <div class="content">
                <div class="action-bar">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Student" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
                    
                    <div class="search-box">
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name, email, or course..." CssClass="search-input"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" />
                    </div>
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

                <div class="stats">
                    <asp:Label ID="lblCount" runat="server" Text="Total Students: 0" CssClass="student-count"></asp:Label>
                </div>

                <asp:GridView ID="gvStudents" runat="server" 
                    AutoGenerateColumns="False" 
                    CssClass="grid-view"
                    OnRowCommand="gvStudents_RowCommand"
                    DataKeyNames="Id"
                    EmptyDataText="No students found.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Course" HeaderText="Course" />
                        <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrollment Date" DataFormatString="{0:MMM dd, yyyy}" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" 
                                    CommandName="EditStudent" 
                                    CommandArgument='<%# Eval("Id") %>' 
                                    CssClass="btn btn-edit" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                                    CommandName="DeleteStudent" 
                                    CommandArgument='<%# Eval("Id") %>' 
                                    CssClass="btn btn-delete" 
                                    OnClientClick="return confirm('Are you sure you want to delete this student?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <footer>
                <p>&copy; 2026 Student Management System - Demonstrating LINQ to List Operations</p>
            </footer>
        </div>
    </form>
</body>
</html>
