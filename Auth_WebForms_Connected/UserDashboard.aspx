<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="Auth_WebForms_Connected.UserDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Dashboard - Connection-Oriented Auth System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-info">
            <div class="container-fluid">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-tachometer-alt"></i> User Dashboard (Connected)
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
                    <div class="card shadow-sm border-info">
                        <div class="card-header bg-info text-white">
                            <h4 class="mb-0">
                                <i class="fas fa-user"></i> 
                                Welcome, <asp:Label ID="lblFullName" runat="server" />!
                            </h4>
                        </div>
                        <div class="card-body">
                            <p class="lead">
                                This dashboard uses <strong>Connection-Oriented ADO.NET</strong> with manual connection management.
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- User Profile & Activity -->
            <div class="row mt-4">
                <div class="col-md-4">
                    <div class="card shadow-sm">
                        <div class="card-header bg-success text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-id-badge"></i> Profile Information
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="text-center mb-3">
                                <i class="fas fa-user-circle fa-5x text-success"></i>
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
                                            <asp:Label ID="lblRoles" runat="server" CssClass="badge bg-success" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-8">
                    <div class="card shadow-sm">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-plug"></i> Connection-Oriented Features
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-database fa-2x text-primary mb-2"></i>
                                            <h6>SqlConnection</h6>
                                            <p class="mb-0 small">Manual connection management</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-code fa-2x text-success mb-2"></i>
                                            <h6>SqlCommand</h6>
                                            <p class="mb-0 small">Direct database commands</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-stream fa-2x text-info mb-2"></i>
                                            <h6>SqlDataReader</h6>
                                            <p class="mb-0 small">Forward-only data reading</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <div class="card bg-light">
                                        <div class="card-body text-center">
                                            <i class="fas fa-lock fa-2x text-danger mb-2"></i>
                                            <h6>Security</h6>
                                            <p class="mb-0 small">SHA512 & Forms Auth</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Technical Details -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow-sm border-warning">
                        <div class="card-header bg-warning text-dark">
                            <h5 class="mb-0">
                                <i class="fas fa-code"></i> Connection-Oriented Implementation Details
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6 class="text-primary">
                                        <i class="fas fa-check-circle"></i> Key Characteristics:
                                    </h6>
                                    <ul class="small">
                                        <li>Manual <code>connection.Open()</code> and <code>connection.Close()</code></li>
                                        <li><code>SqlDataReader</code> for forward-only data access</li>
                                        <li><code>ExecuteReader()</code> for SELECT queries</li>
                                        <li><code>ExecuteNonQuery()</code> for INSERT/UPDATE/DELETE</li>
                                        <li><code>ExecuteScalar()</code> for single value retrieval</li>
                                        <li>Connection stays open during operations</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-success">
                                        <i class="fas fa-star"></i> Advantages:
                                    </h6>
                                    <ul class="small">
                                        <li>Better performance for large data streaming</li>
                                        <li>Lower memory footprint</li>
                                        <li>Real-time data access</li>
                                        <li>Ideal for forward-only scenarios</li>
                                        <li>Direct database connection control</li>
                                        <li>Efficient for read-heavy operations</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Footer -->
        <footer class="bg-light text-center text-muted py-4 mt-5">
            <div class="container">
                <p class="mb-0">
                    <i class="fas fa-plug"></i> Connection-Oriented Architecture - Direct Database Access
                </p>
            </div>
        </footer>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
