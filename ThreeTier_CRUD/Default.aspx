<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ThreeTier_CRUD.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>3-Tier Architecture CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-layer-group me-2"></i>3-Tier CRUD System
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" href="Default.aspx">
                                <i class="fas fa-home me-1"></i>Home
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Employees/EmployeeList.aspx">
                                <i class="fas fa-users me-1"></i>Employees
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Departments/DepartmentList.aspx">
                                <i class="fas fa-building me-1"></i>Departments
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="hero-section bg-primary text-white py-5">
            <div class="container text-center">
                <h1 class="display-4 mb-3">
                    <i class="fas fa-layer-group me-3"></i>3-Tier Architecture
                </h1>
                <p class="lead">Employee Management System with Proper Separation of Concerns</p>
                <p class="mt-4">Presentation Layer → Business Logic Layer → Data Access Layer</p>
            </div>
        </div>

        <div class="container mt-5">
            <div class="row">
                <div class="col-md-4 mb-4">
                    <div class="card shadow h-100 tier-card">
                        <div class="card-body text-center">
                            <div class="tier-icon bg-primary text-white mb-3">
                                <i class="fas fa-desktop fa-2x"></i>
                            </div>
                            <h5 class="card-title">Presentation Layer</h5>
                            <p class="card-text">ASP.NET Web Forms UI that interacts with users. Calls BLL only, never DAL directly.</p>
                            <ul class="list-unstyled text-start">
                                <li><i class="fas fa-check text-success me-2"></i>Default.aspx</li>
                                <li><i class="fas fa-check text-success me-2"></i>EmployeeList.aspx</li>
                                <li><i class="fas fa-check text-success me-2"></i>DepartmentList.aspx</li>
                                <li><i class="fas fa-check text-success me-2"></i>Add/Edit/Details pages</li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 mb-4">
                    <div class="card shadow h-100 tier-card">
                        <div class="card-body text-center">
                            <div class="tier-icon bg-success text-white mb-3">
                                <i class="fas fa-cogs fa-2x"></i>
                            </div>
                            <h5 class="card-title">Business Logic Layer (BLL)</h5>
                            <p class="card-text">Contains validation rules, business logic, and workflow coordination.</p>
                            <ul class="list-unstyled text-start">
                                <li><i class="fas fa-check text-success me-2"></i>EmployeeBLL.cs</li>
                                <li><i class="fas fa-check text-success me-2"></i>DepartmentBLL.cs</li>
                                <li><i class="fas fa-check text-success me-2"></i>Validation rules</li>
                                <li><i class="fas fa-check text-success me-2"></i>Business constraints</li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 mb-4">
                    <div class="card shadow h-100 tier-card">
                        <div class="card-body text-center">
                            <div class="tier-icon bg-info text-white mb-3">
                                <i class="fas fa-database fa-2x"></i>
                            </div>
                            <h5 class="card-title">Data Access Layer (DAL)</h5>
                            <p class="card-text">Pure database operations with ADO.NET. No business logic, only CRUD operations.</p>
                            <ul class="list-unstyled text-start">
                                <li><i class="fas fa-check text-success me-2"></i>EmployeeDAL.cs</li>
                                <li><i class="fas fa-check text-success me-2"></i>DepartmentDAL.cs</li>
                                <li><i class="fas fa-check text-success me-2"></i>DBHelper.cs</li>
                                <li><i class="fas fa-check text-success me-2"></i>SQL operations</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-md-6 mb-4">
                    <div class="card shadow">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-users me-2"></i>Employee Management
                            </h5>
                        </div>
                        <div class="card-body">
                            <p>Manage employee records with full CRUD operations:</p>
                            <ul>
                                <li>View all employees</li>
                                <li>Add new employees</li>
                                <li>Edit employee details</li>
                                <li>View employee information</li>
                                <li>Delete employees</li>
                                <li>Search and filter</li>
                            </ul>
                            <a href="Employees/EmployeeList.aspx" class="btn btn-primary">
                                <i class="fas fa-arrow-right me-2"></i>Go to Employees
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 mb-4">
                    <div class="card shadow">
                        <div class="card-header bg-success text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-building me-2"></i>Department Management
                            </h5>
                        </div>
                        <div class="card-body">
                            <p>Manage department records with CRUD operations:</p>
                            <ul>
                                <li>View all departments</li>
                                <li>Add new departments</li>
                                <li>Edit department details</li>
                                <li>View employee count per department</li>
                                <li>Delete departments (business rules apply)</li>
                            </ul>
                            <a href="Departments/DepartmentList.aspx" class="btn btn-success">
                                <i class="fas fa-arrow-right me-2"></i>Go to Departments
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-dark text-white">
                            <h5 class="mb-0">
                                <i class="fas fa-info-circle me-2"></i>Architecture Benefits
                            </h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Separation of Concerns</h6>
                                    <p class="small">Each layer has a specific responsibility and doesn't know about implementation details of other layers.</p>
                                    
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Maintainability</h6>
                                    <p class="small">Changes in one layer don't affect others. Business logic changes don't require UI modifications.</p>
                                    
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Testability</h6>
                                    <p class="small">Each layer can be tested independently. Business logic can be unit tested without UI.</p>
                                </div>
                                <div class="col-md-6">
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Reusability</h6>
                                    <p class="small">Business logic and data access can be reused by different presentation layers (Web, Mobile, Desktop).</p>
                                    
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Scalability</h6>
                                    <p class="small">Layers can be deployed on different servers. Database can be scaled independently.</p>
                                    
                                    <h6><i class="fas fa-check-circle text-success me-2"></i>Security</h6>
                                    <p class="small">Presentation layer cannot bypass business rules. All operations go through BLL validation.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <footer class="bg-dark text-white text-center py-4 mt-5">
            <div class="container">
                <p class="mb-0">3-Tier Architecture CRUD Application - ASP.NET Web Forms</p>
                <small>Presentation Layer → Business Logic Layer → Data Access Layer</small>
            </div>
        </footer>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
