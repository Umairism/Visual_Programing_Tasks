<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Auth_WebForms_Connected.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home - Connection-Oriented Auth System</title>
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
                    <i class="fas fa-plug"></i> Connected Auth System
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" href="Default.aspx">
                                <i class="fas fa-home"></i> Home
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="UserDashboard.aspx">
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
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" 
                               data-bs-toggle="dropdown">
                                <i class="fas fa-user-circle"></i>
                                <asp:Label ID="lblUserName" runat="server" />
                            </a>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li>
                                    <a class="dropdown-item" href="#">
                                        <i class="fas fa-id-badge"></i>
                                        <asp:Label ID="lblUserRole" runat="server" />
                                    </a>
                                </li>
                                <li><hr class="dropdown-divider" /></li>
                                <li>
                                    <a class="dropdown-item" href="Logout.aspx">
                                        <i class="fas fa-sign-out-alt"></i> Logout
                                    </a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- Main Content -->
        <div class="container mt-5">
            <div class="row">
                <div class="col-12">
                    <div class="jumbotron bg-light p-5 rounded shadow">
                        <h1 class="display-4">
                            <i class="fas fa-check-circle text-success"></i>
                            Welcome, <asp:Label ID="lblFullName" runat="server" CssClass="text-success" />!
                        </h1>
                        <p class="lead">
                            You are successfully authenticated using ADO.NET Connection-Oriented Approach.
                        </p>
                        <hr class="my-4" />
                        <p>
                            This application demonstrates authentication and authorization using:
                        </p>
                        <ul class="list-unstyled">
                            <li><i class="fas fa-check text-success"></i> Forms Authentication</li>
                            <li><i class="fas fa-check text-success"></i> Role-based Authorization</li>
                            <li><i class="fas fa-check text-success"></i> ADO.NET SqlConnection (Connection-Oriented)</li>
                            <li><i class="fas fa-check text-success"></i> SqlCommand & SqlDataReader</li>
                            <li><i class="fas fa-check text-success"></i> Manual Connection Management</li>
                            <li><i class="fas fa-check text-success"></i> SHA512 Password Hashing</li>
                        </ul>
                    </div>
                </div>
            </div>

            <!-- User Info Cards -->
            <div class="row mt-4">
                <div class="col-md-4">
                    <div class="card shadow-sm border-success">
                        <div class="card-body text-center">
                            <i class="fas fa-user fa-3x text-success mb-3"></i>
                            <h5 class="card-title">User Information</h5>
                            <p class="card-text">
                                <strong>Username:</strong> <asp:Label ID="lblUsername" runat="server" /><br />
                                <strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" />
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="card shadow-sm border-success">
                        <div class="card-body text-center">
                            <i class="fas fa-shield-alt fa-3x text-success mb-3"></i>
                            <h5 class="card-title">Authorization</h5>
                            <p class="card-text">
                                <strong>Your Roles:</strong><br />
                                <asp:Label ID="lblRoles" runat="server" CssClass="badge bg-success fs-6" />
                            </p>
                            <asp:Panel ID="pnlAdminAccess" runat="server" Visible="false">
                                <p class="text-success">
                                    <i class="fas fa-check-circle"></i> You have Admin access
                                </p>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="card shadow-sm border-success">
                        <div class="card-body text-center">
                            <i class="fas fa-database fa-3x text-warning mb-3"></i>
                            <h5 class="card-title">Technology</h5>
                            <p class="card-text">
                                <span class="badge bg-success">SqlConnection</span><br />
                                <span class="badge bg-info">SqlCommand</span><br />
                                <span class="badge bg-primary">SqlDataReader</span><br />
                                <span class="badge bg-warning text-dark">Connected</span>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Connection-Oriented Info -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow-sm border-info">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-plug"></i> Connection-Oriented Architecture
                            </h5>
                        </div>
                        <div class="card-body">
                            <p class="mb-2"><strong>What is Connection-Oriented Approach?</strong></p>
                            <p>In connection-oriented (connected) architecture:</p>
                            <ul>
                                <li><i class="fas fa-arrow-right text-info"></i> Connections are <strong>manually opened</strong> using <code>connection.Open()</code></li>
                                <li><i class="fas fa-arrow-right text-info"></i> Data is read in <strong>real-time</strong> using <code>SqlDataReader</code></li>
                                <li><i class="fas fa-arrow-right text-info"></i> Connection remains <strong>open during operations</strong></li>
                                <li><i class="fas fa-arrow-right text-info"></i> Must <strong>manually close</strong> using <code>connection.Close()</code> or dispose pattern</li>
                                <li><i class="fas fa-arrow-right text-info"></i> Best for <strong>real-time streaming</strong> and forward-only data reading</li>
                                <li><i class="fas fa-arrow-right text-info"></i> Uses <code>ExecuteReader()</code>, <code>ExecuteNonQuery()</code>, <code>ExecuteScalar()</code></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Footer -->
        <footer class="bg-light text-center text-muted py-4 mt-5">
            <div class="container">
                <p class="mb-0">
                    <i class="fas fa-plug"></i> 2026 Connection-Oriented Auth System - 
                    ADO.NET Connected Architecture with ASP.NET Web Forms
                </p>
            </div>
        </footer>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
