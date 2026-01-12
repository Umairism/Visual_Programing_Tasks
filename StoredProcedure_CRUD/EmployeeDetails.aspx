<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeDetails.aspx.cs" Inherits="StoredProcedure_CRUD.EmployeeDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Details - Stored Procedures CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="EmployeeList.aspx">
                    <i class="fas fa-users me-2"></i>Employee Management
                </a>
                <span class="navbar-text text-white">Employee Details</span>
            </div>
        </nav>

        <div class="container mt-4">
            <div class="row">
                <div class="col-md-8 offset-md-2">
                    <div class="card shadow">
                        <div class="card-header bg-info text-white">
                            <h4 class="mb-0">
                                <i class="fas fa-id-card me-2"></i>Employee Details
                            </h4>
                            <small>Retrieved using sp_GetEmployeeById stored procedure</small>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger">
                                <asp:Label ID="lblMessage" runat="server" />
                            </asp:Panel>

                            <asp:Panel ID="pnlDetails" runat="server">
                                <div class="row mb-4">
                                    <div class="col-12 text-center">
                                        <div class="avatar-circle-large bg-primary text-white mx-auto mb-3">
                                            <asp:Label ID="lblInitials" runat="server" CssClass="h1" />
                                        </div>
                                        <h3><asp:Label ID="lblFullName" runat="server" /></h3>
                                        <p class="text-muted">
                                            <asp:Label ID="lblPosition" runat="server" />
                                        </p>
                                        <asp:Panel ID="pnlStatus" runat="server" CssClass="badge">
                                            <asp:Label ID="lblStatus" runat="server" />
                                        </asp:Panel>
                                    </div>
                                </div>

                                <hr />

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-id-badge text-primary me-2"></i>
                                            <strong>Employee ID:</strong>
                                            <span class="ms-2"><asp:Label ID="lblEmployeeId" runat="server" /></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-envelope text-primary me-2"></i>
                                            <strong>Email:</strong>
                                            <span class="ms-2"><asp:Label ID="lblEmail" runat="server" /></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-phone text-primary me-2"></i>
                                            <strong>Phone:</strong>
                                            <span class="ms-2"><asp:Label ID="lblPhone" runat="server" /></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-building text-primary me-2"></i>
                                            <strong>Department:</strong>
                                            <span class="ms-2"><asp:Label ID="lblDepartment" runat="server" /></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-dollar-sign text-success me-2"></i>
                                            <strong>Salary:</strong>
                                            <span class="ms-2 text-success fw-bold"><asp:Label ID="lblSalary" runat="server" /></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-calendar text-primary me-2"></i>
                                            <strong>Hire Date:</strong>
                                            <span class="ms-2"><asp:Label ID="lblHireDate" runat="server" /></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-clock text-primary me-2"></i>
                                            <strong>Created Date:</strong>
                                            <span class="ms-2"><asp:Label ID="lblCreatedDate" runat="server" /></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="detail-item">
                                            <i class="fas fa-edit text-primary me-2"></i>
                                            <strong>Modified Date:</strong>
                                            <span class="ms-2"><asp:Label ID="lblModifiedDate" runat="server" /></span>
                                        </div>
                                    </div>
                                </div>

                                <hr />

                                <div class="d-grid gap-2 d-md-flex justify-content-md-between">
                                    <a href="EmployeeList.aspx" class="btn btn-secondary">
                                        <i class="fas fa-arrow-left me-2"></i>Back to List
                                    </a>
                                    <div>
                                        <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-warning me-2">
                                            <i class="fas fa-edit me-2"></i>Edit
                                        </asp:HyperLink>
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                            OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this employee?');" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <!-- Additional Information -->
                    <div class="card shadow mt-3">
                        <div class="card-header bg-secondary text-white">
                            <h6 class="mb-0">
                                <i class="fas fa-database me-2"></i>Stored Procedure Information
                            </h6>
                        </div>
                        <div class="card-body">
                            <p class="small mb-1"><strong>Stored Procedure Used:</strong> <code>sp_GetEmployeeById</code></p>
                            <p class="small mb-1"><strong>Parameter:</strong> <code>@EmployeeId</code></p>
                            <p class="small mb-0"><strong>Return Type:</strong> Single employee record with all details</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
