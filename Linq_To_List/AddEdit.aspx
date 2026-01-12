<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEdit.aspx.cs" Inherits="AddEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add/Edit Student</title>
    <link href="Styles/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <header>
                <h1>
                    <asp:Label ID="lblTitle" runat="server" Text="Add New Student"></asp:Label>
                </h1>
                <p>Fill in the student details below</p>
            </header>

            <div class="content">
                <div class="form-container">
                    <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

                    <div class="form-group">
                        <label>Name:</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-input" placeholder="Enter student name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                            ControlToValidate="txtName" 
                            ErrorMessage="Name is required" 
                            CssClass="error-message"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Email:</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="Enter email address" TextMode="Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                            ControlToValidate="txtEmail" 
                            ErrorMessage="Email is required" 
                            CssClass="error-message"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Invalid email format"
                            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                            CssClass="error-message"
                            Display="Dynamic">
                        </asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <label>Course:</label>
                        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-input">
                            <asp:ListItem Value="">-- Select Course --</asp:ListItem>
                            <asp:ListItem Value="Computer Science">Computer Science</asp:ListItem>
                            <asp:ListItem Value="Business Administration">Business Administration</asp:ListItem>
                            <asp:ListItem Value="Engineering">Engineering</asp:ListItem>
                            <asp:ListItem Value="Medicine">Medicine</asp:ListItem>
                            <asp:ListItem Value="Arts">Arts</asp:ListItem>
                            <asp:ListItem Value="Law">Law</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCourse" runat="server" 
                            ControlToValidate="ddlCourse" 
                            ErrorMessage="Please select a course" 
                            CssClass="error-message"
                            Display="Dynamic"
                            InitialValue="">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Enrollment Date:</label>
                        <asp:TextBox ID="txtEnrollmentDate" runat="server" CssClass="form-input" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEnrollmentDate" runat="server" 
                            ControlToValidate="txtEnrollmentDate" 
                            ErrorMessage="Enrollment date is required" 
                            CssClass="error-message"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Phone:</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input" placeholder="Enter phone number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                            ControlToValidate="txtPhone" 
                            ErrorMessage="Phone is required" 
                            CssClass="error-message"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="button-group">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>

            <footer>
                <p>&copy; 2026 Student Management System</p>
            </footer>
        </div>
    </form>
</body>
</html>
