<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="Auth_WebForms.UserDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Dashboard - Authentication System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-success">
            <div class="container-fluid">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-tachometer-alt"></i> User Dashboard
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="Default.aspx">
                                <i class="fas fa-home"></i> Home
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="UserDashboard.aspx">
                                <i class="fas fa-tachometer-alt"></i> Dashboard
                            </a>
                        </li>
                        <li class="nav-item" runat="server" id="liAdminPanel">
                            <a class="nav-link" href="AdminPanel.aspx">
                                <i class="fas fa-user-shield"></i> Admin Panel
                            </a>
                        </li>
                    </ul>
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <a class="nav-link" href="#">
                                <i class="fas fa-user-circle"></i>
                                <asp:Label ID="lblNavUsername" runat="server" />
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Logout.aspx">
                                <i class="fas fa-sign-out-alt"></i> Logout
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- Main Content -->
        <div class="container mt-4">
            <!-- Welcome Section -->
            <div class="row">
                <div class="col-12">
                    <div class="card shadow-sm border-success">
                        <div class="card-header bg-success text-white">
                            <h4 class="mb-0">
                                <i class="fas fa-user"></i> 
                                Welcome to Your Dashboard, <asp:Label ID="lblFullName" runat="server" />!
                            </h4>
                        </div>
                        <div class="card-body">
                            <p class="lead">
                                This is your personalized dashboard. You are authorized to view this page 
                                based on your role assignment.
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- User Profile Card -->
            <div class="row mt-4">
                <div class="col-md-4">
                    <div class="card shadow-sm">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-id-badge"></i> Profile Information
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="text-center mb-3">
                                <i class="fas fa-user-circle fa-5x text-primary"></i>
                            </div>
                            <table class="table table-sm">
                                <tbody>
                                    <tr>
                                        <th width="40%">Username:</th>
                                        <td><asp:Label ID="lblUsername" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>Full Name:</th>
                                        <td><asp:Label ID="lblFullNameProfile" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>Email:</th>
                                        <td><asp:Label ID="lblEmail" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>Role(s):</th>
                                        <td>
                                            <asp:Label ID="lblRoles" runat="server" CssClass="badge bg-info" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Status:</th>
                                        <td>
                                            <span class="badge bg-success">
                                                <i class="fas fa-check-circle"></i> Active
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Activity Summary -->
                <div class="col-md-8">
                    <div class="card shadow-sm">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-chart-line"></i> Activity Summary
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-calendar-check fa-2x text-primary mb-2"></i>
                                            <h6>Last Login</h6>
                                            <p class="mb-0">
                                                <asp:Label ID="lblLastLogin" runat="server" CssClass="fw-bold" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-shield-alt fa-2x text-success mb-2"></i>
                                            <h6>Authentication Method</h6>
                                            <p class="mb-0 fw-bold">Forms Authentication</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-database fa-2x text-warning mb-2"></i>
                                            <h6>Data Access</h6>
                                            <p class="mb-0 fw-bold">ADO.NET Connectionless</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-lock fa-2x text-danger mb-2"></i>
                                            <h6>Security</h6>
                                            <p class="mb-0 fw-bold">SHA512 Hashing</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Features & Capabilities -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow-sm">
                        <div class="card-header bg-secondary text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-cogs"></i> Your Access & Capabilities
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6><i class="fas fa-check-circle text-success"></i> Available Features:</h6>
                                    <ul class="list-unstyled ms-4">
                                        <li><i class="fas fa-arrow-right text-primary"></i> View Dashboard</li>
                                        <li><i class="fas fa-arrow-right text-primary"></i> View Profile Information</li>
                                        <li><i class="fas fa-arrow-right text-primary"></i> Access Home Page</li>
                                        <li><i class="fas fa-arrow-right text-primary"></i> Secure Logout</li>
                                        <li runat="server" id="liAdminFeature" visible="false">
                                            <i class="fas fa-arrow-right text-danger"></i> 
                                            <strong>Access Admin Panel</strong>
                                        </li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6><i class="fas fa-shield-alt text-success"></i> Authorization Details:</h6>
                                    <ul class="list-unstyled ms-4">
                                        <li>
                                            <i class="fas fa-check text-success"></i> 
                                            <strong>Role-based Access:</strong> Configured in Web.config
                                        </li>
                                        <li>
                                            <i class="fas fa-check text-success"></i> 
                                            <strong>Session Management:</strong> Secure session handling
                                        </li>
                                        <li>
                                            <i class="fas fa-check text-success"></i> 
                                            <strong>Forms Authentication:</strong> Cookie-based authentication
                                        </li>
                                        <li>
                                            <i class="fas fa-check text-success"></i> 
                                            <strong>ADO.NET:</strong> DataAdapter & DataSet approach
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Technical Information -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow-sm border-info">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-code"></i> Technical Implementation
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <h6 class="text-primary">
                                        <i class="fas fa-database"></i> Data Access Layer
                                    </h6>
                                    <ul class="small">
                                        <li>ADO.NET Connectionless approach</li>
                                        <li>SqlDataAdapter for data retrieval</li>
                                        <li>DataSet & DataTable for disconnected data</li>
                                        <li>Stored Procedures for operations</li>
                                    </ul>
                                </div>
                                <div class="col-md-4">
                                    <h6 class="text-success">
                                        <i class="fas fa-lock"></i> Authentication
                                    </h6>
                                    <ul class="small">
                                        <li>Forms Authentication</li>
                                        <li>FormsAuthenticationTicket</li>
                                        <li>SHA512 password hashing</li>
                                        <li>Encrypted authentication cookie</li>
                                    </ul>
                                </div>
                                <div class="col-md-4">
                                    <h6 class="text-warning">
                                        <i class="fas fa-user-shield"></i> Authorization
                                    </h6>
                                    <ul class="small">
                                        <li>Role-based authorization</li>
                                        <li>Web.config location elements</li>
                                        <li>User.IsInRole() checks</li>
                                        <li>Multi-role support</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Quick Actions -->
            <div class="row mt-4 mb-5">
                <div class="col-12">
                    <div class="card shadow-sm">
                        <div class="card-header bg-dark text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-bolt"></i> Quick Actions
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-4 mb-2">
                                    <a href="Default.aspx" class="btn btn-outline-primary w-100">
                                        <i class="fas fa-home"></i> Go to Home
                                    </a>
                                </div>
                                <div class="col-md-4 mb-2" runat="server" id="divAdminAction">
                                    <a href="AdminPanel.aspx" class="btn btn-outline-danger w-100">
                                        <i class="fas fa-user-shield"></i> Open Admin Panel
                                    </a>
                                </div>
                                <div class="col-md-4 mb-2">
                                    <a href="Logout.aspx" class="btn btn-outline-secondary w-100">
                                        <i class="fas fa-sign-out-alt"></i> Logout
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Footer -->
        <footer class="bg-light text-center text-muted py-4">
            <div class="container">
                <p class="mb-0">
                    <i class="fas fa-shield-alt"></i> User Dashboard - 
                    Protected with Forms Authentication & Role-based Authorization
                </p>
            </div>
        </footer>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
