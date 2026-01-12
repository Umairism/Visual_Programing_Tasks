<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Auth_WebForms.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Authentication System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body class="auth-page">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center mt-5">
                <div class="col-md-5">
                    <div class="card shadow-lg">
                        <div class="card-header bg-primary text-white text-center py-4">
                            <i class="fas fa-lock fa-3x mb-3"></i>
                            <h3>Authentication System</h3>
                            <p class="mb-0">ADO.NET Connectionless Approach</p>
                        </div>
                        <div class="card-body p-5">
                            <h4 class="text-center mb-4">Sign In</h4>
                            
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                                <asp:Label ID="lblMessage" runat="server" />
                            </asp:Panel>

                            <div class="mb-3">
                                <label for="txtUsername" class="form-label">
                                    <i class="fas fa-user"></i> Username
                                </label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" 
                                    placeholder="Enter username" />
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                    ControlToValidate="txtUsername"
                                    ErrorMessage="Username is required" 
                                    Display="Dynamic"
                                    CssClass="text-danger small" />
                            </div>

                            <div class="mb-3">
                                <label for="txtPassword" class="form-label">
                                    <i class="fas fa-key"></i> Password
                                </label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                                    CssClass="form-control" placeholder="Enter password" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                    ControlToValidate="txtPassword"
                                    ErrorMessage="Password is required" 
                                    Display="Dynamic"
                                    CssClass="text-danger small" />
                            </div>

                            <div class="mb-3 form-check">
                                <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkRememberMe">
                                    Remember me
                                </label>
                            </div>

                            <div class="d-grid gap-2 mb-3">
                                <asp:Button ID="btnLogin" runat="server" Text="Sign In" 
                                    CssClass="btn btn-primary btn-lg" OnClick="btnLogin_Click" />
                            </div>

                            <hr />

                            <div class="text-center">
                                <p class="mb-0">Don't have an account?</p>
                                <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx" 
                                    CssClass="btn btn-outline-secondary mt-2">
                                    <i class="fas fa-user-plus"></i> Create New Account
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="card-footer text-center text-muted">
                            <small>
                                <i class="fas fa-info-circle"></i> 
                                Default: admin/Admin@123 or john.doe/User@123
                            </small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
