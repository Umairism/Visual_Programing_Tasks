<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Auth_WebForms.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register - Authentication System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body class="auth-page">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center mt-5">
                <div class="col-md-6">
                    <div class="card shadow-lg">
                        <div class="card-header bg-success text-white text-center py-4">
                            <i class="fas fa-user-plus fa-3x mb-3"></i>
                            <h3>Create Account</h3>
                            <p class="mb-0">Register with Connectionless ADO.NET</p>
                        </div>
                        <div class="card-body p-4">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                                <asp:Label ID="lblMessage" runat="server" />
                            </asp:Panel>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label for="txtUsername" class="form-label">
                                        <i class="fas fa-user"></i> Username *
                                    </label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" 
                                        placeholder="Enter username" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                        ControlToValidate="txtUsername"
                                        ErrorMessage="Username is required" 
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                    <asp:RegularExpressionValidator ID="revUsername" runat="server"
                                        ControlToValidate="txtUsername"
                                        ValidationExpression="^[a-zA-Z0-9._]{3,50}$"
                                        ErrorMessage="Username must be 3-50 characters (letters, numbers, . or _)"
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label for="txtFullName" class="form-label">
                                        <i class="fas fa-id-card"></i> Full Name *
                                    </label>
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" 
                                        placeholder="Enter full name" MaxLength="100" />
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                                        ControlToValidate="txtFullName"
                                        ErrorMessage="Full name is required" 
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">
                                    <i class="fas fa-envelope"></i> Email Address *
                                </label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                    placeholder="Enter email address" TextMode="Email" MaxLength="100" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="Email is required" 
                                    Display="Dynamic"
                                    CssClass="text-danger small" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                    ControlToValidate="txtEmail"
                                    ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                    ErrorMessage="Invalid email format"
                                    Display="Dynamic"
                                    CssClass="text-danger small" />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label for="txtPassword" class="form-label">
                                        <i class="fas fa-key"></i> Password *
                                    </label>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                                        CssClass="form-control" placeholder="Enter password" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                        ControlToValidate="txtPassword"
                                        ErrorMessage="Password is required" 
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                    <small class="form-text text-muted">
                                        Min 8 chars, 1 uppercase, 1 lowercase, 1 digit, 1 special char
                                    </small>
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label for="txtConfirmPassword" class="form-label">
                                        <i class="fas fa-key"></i> Confirm Password *
                                    </label>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                                        CssClass="form-control" placeholder="Confirm password" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                                        ControlToValidate="txtConfirmPassword"
                                        ErrorMessage="Confirm password is required" 
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                    <asp:CompareValidator ID="cvPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtPassword"
                                        ErrorMessage="Passwords do not match"
                                        Display="Dynamic"
                                        CssClass="text-danger small" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="ddlRole" class="form-label">
                                    <i class="fas fa-user-tag"></i> Role
                                </label>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="User" Selected="True">User</asp:ListItem>
                                    <asp:ListItem Value="Guest">Guest</asp:ListItem>
                                </asp:DropDownList>
                                <small class="form-text text-muted">
                                    Select your account type
                                </small>
                            </div>

                            <div class="mb-3 form-check">
                                <asp:CheckBox ID="chkAgree" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkAgree">
                                    I agree to the Terms and Conditions *
                                </label>
                                <asp:CustomValidator ID="cvAgree" runat="server"
                                    ErrorMessage="You must agree to the terms"
                                    Display="Dynamic"
                                    CssClass="text-danger small d-block"
                                    OnServerValidate="cvAgree_ServerValidate" />
                            </div>

                            <div class="d-grid gap-2 mb-3">
                                <asp:Button ID="btnRegister" runat="server" Text="Create Account" 
                                    CssClass="btn btn-success btn-lg" OnClick="btnRegister_Click" />
                            </div>

                            <hr />

                            <div class="text-center">
                                <p class="mb-0">Already have an account?</p>
                                <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" 
                                    CssClass="btn btn-outline-primary mt-2">
                                    <i class="fas fa-sign-in-alt"></i> Sign In
                                </asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
