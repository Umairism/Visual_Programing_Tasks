<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Auth_WebForms_Connected.AdminPanel" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Panel - Connection-Oriented Auth System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-danger">
            <div class="container-fluid">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-user-shield"></i> Admin Panel (Connected)
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
                            <a class="nav-link" href="UserDashboard.aspx">
                                <i class="fas fa-tachometer-alt"></i> Dashboard
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="AdminPanel.aspx">
                                <i class="fas fa-user-shield"></i> Admin Panel
                            </a>
                        </li>
                    </ul>
                    <ul class="navbar-nav">
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
        <div class="container-fluid mt-4">
            <!-- Admin Header -->
            <div class="row">
                <div class="col-12">
                    <div class="alert alert-danger" role="alert">
                        <h4 class="alert-heading">
                            <i class="fas fa-exclamation-triangle"></i> Administrator Access Only
                        </h4>
                        <p class="mb-0">
                            Using <strong>Connection-Oriented ADO.NET</strong> with SqlConnection, SqlCommand & SqlDataReader.
                            All database operations use manual connection management.
                        </p>
                    </div>
                </div>
            </div>

            <!-- Statistics Cards -->
            <div class="row mb-4">
                <div class="col-md-3">
                    <div class="card bg-primary text-white shadow">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <h6 class="text-uppercase mb-1">Total Users</h6>
                                    <h2 class="mb-0">
                                        <asp:Label ID="lblTotalUsers" runat="server" Text="0" />
                                    </h2>
                                </div>
                                <i class="fas fa-users fa-3x opacity-50"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card bg-success text-white shadow">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <h6 class="text-uppercase mb-1">Active Users</h6>
                                    <h2 class="mb-0">
                                        <asp:Label ID="lblActiveUsers" runat="server" Text="0" />
                                    </h2>
                                </div>
                                <i class="fas fa-user-check fa-3x opacity-50"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card bg-warning text-white shadow">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <h6 class="text-uppercase mb-1">Inactive Users</h6>
                                    <h2 class="mb-0">
                                        <asp:Label ID="lblInactiveUsers" runat="server" Text="0" />
                                    </h2>
                                </div>
                                <i class="fas fa-user-slash fa-3x opacity-50"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card bg-danger text-white shadow">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <h6 class="text-uppercase mb-1">Locked Users</h6>
                                    <h2 class="mb-0">
                                        <asp:Label ID="lblLockedUsers" runat="server" Text="0" />
                                    </h2>
                                </div>
                                <i class="fas fa-lock fa-3x opacity-50"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- User Management -->
            <div class="row">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-danger text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-users-cog"></i> User Management (SqlDataReader)
                                <span class="float-end">
                                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" 
                                        CssClass="btn btn-sm btn-light" OnClick="btnRefresh_Click" />
                                </span>
                            </h5>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                                <asp:Label ID="lblMessage" runat="server" />
                            </asp:Panel>

                            <div class="table-responsive">
                                <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover">
                                            <thead class="table-dark">
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Username</th>
                                                    <th>Email</th>
                                                    <th>Full Name</th>
                                                    <th>Roles</th>
                                                    <th>Status</th>
                                                    <th>Locked</th>
                                                    <th>Created</th>
                                                    <th>Last Login</th>
                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("UserId") %></td>
                                            <td><%# Eval("Username") %></td>
                                            <td><%# Eval("Email") %></td>
                                            <td><%# Eval("FullName") %></td>
                                            <td><%# Eval("Roles") %></td>
                                            <td>
                                                <%# Convert.ToBoolean(Eval("IsActive")) 
                                                    ? "<span class='badge bg-success'>Active</span>" 
                                                    : "<span class='badge bg-secondary'>Inactive</span>" %>
                                            </td>
                                            <td>
                                                <%# Convert.ToBoolean(Eval("IsLockedOut")) 
                                                    ? "<span class='badge bg-danger'><i class='fas fa-lock'></i></span>" 
                                                    : "<span class='badge bg-success'><i class='fas fa-unlock'></i></span>" %>
                                            </td>
                                            <td><%# Convert.ToDateTime(Eval("CreatedDate")).ToString("MMM dd, yyyy") %></td>
                                            <td>
                                                <%# Eval("LastLoginDate") != DBNull.Value 
                                                    ? Convert.ToDateTime(Eval("LastLoginDate")).ToString("MMM dd, yyyy HH:mm") 
                                                    : "Never" %>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnToggleStatus" runat="server" 
                                                    CommandName="ToggleStatus" 
                                                    CommandArgument='<%# Eval("UserId") + "," + Eval("IsActive") %>'
                                                    Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Activate" %>'
                                                    CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-sm btn-warning" : "btn btn-sm btn-success" %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Connection-Oriented Info -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow border-info">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-plug"></i> Connection-Oriented Implementation
                            </h5>
                        </div>
                        <div class="card-body">
                            <p class="mb-2"><strong>This admin panel demonstrates:</strong></p>
                            <ul>
                                <li><i class="fas fa-check text-success"></i> <strong>SqlConnection:</strong> Manual connection management with Open()/Close()</li>
                                <li><i class="fas fa-check text-success"></i> <strong>SqlCommand:</strong> ExecuteReader, ExecuteNonQuery, ExecuteScalar</li>
                                <li><i class="fas fa-check text-success"></i> <strong>SqlDataReader:</strong> Forward-only, read-only data access</li>
                                <li><i class="fas fa-check text-success"></i> <strong>Real-time Data:</strong> Data retrieved directly from database</li>
                                <li><i class="fas fa-check text-success"></i> <strong>Transactions:</strong> Multi-step operations with rollback capability</li>
                                <li><i class="fas fa-check text-success"></i> <strong>Manual Control:</strong> Full control over connection lifecycle</li>
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
                    <i class="fas fa-shield-alt"></i> Admin Panel - Connection-Oriented Architecture
                </p>
            </div>
        </footer>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
