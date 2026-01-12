<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DbCon_CRUD.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Centralized DbCon CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-success">
            <div class="container">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-database me-2"></i>Centralized DbCon CRUD
                </a>
                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" href="Default.aspx"><i class="fas fa-home me-1"></i>Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Products/ProductList.aspx"><i class="fas fa-box me-1"></i>Products</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Categories/CategoryList.aspx"><i class="fas fa-tags me-1"></i>Categories</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="hero-section bg-success text-white py-5">
            <div class="container text-center">
                <h1 class="display-4 mb-3">
                    <i class="fas fa-database me-3"></i>Centralized DbCon Architecture
                </h1>
                <p class="lead">Inventory Management System using Single Database Connection Class</p>
                <p class="mt-4">All database operations through <code class="text-warning">DbCon</code> static class</p>
            </div>
        </div>

        <div class="container mt-5">
            <div class="row">
                <div class="col-md-6 offset-md-3 mb-4">
                    <div class="card shadow architecture-card">
                        <div class="card-body text-center">
                            <div class="architecture-icon bg-success text-white mb-3">
                                <i class="fas fa-sitemap fa-3x"></i>
                            </div>
                            <h5 class="card-title">Centralized DbCon Pattern</h5>
                            <div class="architecture-flow">
                                <div class="flow-box bg-light">
                                    <i class="fas fa-desktop"></i>
                                    <p class="mb-0"><strong>Presentation Layer</strong></p>
                                    <small>Web Forms (.aspx)</small>
                                </div>
                                <div class="flow-arrow">↓ calls</div>
                                <div class="flow-box bg-success text-white">
                                    <i class="fas fa-database"></i>
                                    <p class="mb-0"><strong>DbCon Class</strong></p>
                                    <small>Static utility methods</small>
                                </div>
                                <div class="flow-arrow">↓ executes</div>
                                <div class="flow-box bg-light">
                                    <i class="fas fa-server"></i>
                                    <p class="mb-0"><strong>SQL Server</strong></p>
                                    <small>Database</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-md-6 mb-4">
                    <div class="card shadow h-100">
                        <div class="card-header bg-success text-white">
                            <h5 class="mb-0"><i class="fas fa-box me-2"></i>Product Management</h5>
                        </div>
                        <div class="card-body">
                            <p>Manage product inventory with CRUD operations:</p>
                            <ul>
                                <li>View all products with stock levels</li>
                                <li>Add new products</li>
                                <li>Update product details and prices</li>
                                <li>Delete products</li>
                                <li>Track stock quantities</li>
                                <li>Calculate inventory value</li>
                            </ul>
                            <a href="Products/ProductList.aspx" class="btn btn-success">
                                <i class="fas fa-arrow-right me-2"></i>Manage Products
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 mb-4">
                    <div class="card shadow h-100">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0"><i class="fas fa-tags me-2"></i>Category Management</h5>
                        </div>
                        <div class="card-body">
                            <p>Organize products by categories:</p>
                            <ul>
                                <li>View all categories</li>
                                <li>Add new categories</li>
                                <li>Edit category information</li>
                                <li>See product count per category</li>
                                <li>Manage category status</li>
                            </ul>
                            <a href="Categories/CategoryList.aspx" class="btn btn-info">
                                <i class="fas fa-arrow-right me-2"></i>Manage Categories
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-dark text-white">
                            <h5 class="mb-0"><i class="fas fa-code me-2"></i>DbCon Class Features</h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6 class="text-success"><i class="fas fa-check-circle me-2"></i>Core Methods</h6>
                                    <ul class="list-unstyled">
                                        <li><code>ExecuteNonQuery()</code> - INSERT, UPDATE, DELETE</li>
                                        <li><code>ExecuteScalar()</code> - COUNT, MAX, SUM</li>
                                        <li><code>ExecuteReader()</code> - SELECT with SqlDataReader</li>
                                        <li><code>ExecuteDataTable()</code> - SELECT to DataTable</li>
                                        <li><code>ExecuteDataSet()</code> - Multiple result sets</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-success"><i class="fas fa-check-circle me-2"></i>Helper Methods</h6>
                                    <ul class="list-unstyled">
                                        <li><code>CreateParameter()</code> - SQL parameter creation</li>
                                        <li><code>RecordExists()</code> - Check record existence</li>
                                        <li><code>GetRecordCount()</code> - Count records</li>
                                        <li><code>ExecuteTransaction()</code> - Transaction support</li>
                                        <li><code>TestConnection()</code> - Connection validation</li>
                                    </ul>
                                </div>
                            </div>

                            <div class="alert alert-info mt-3">
                                <h6><i class="fas fa-info-circle me-2"></i>Key Characteristics</h6>
                                <ul class="mb-0">
                                    <li><strong>Static Class:</strong> All methods are static - no instantiation needed</li>
                                    <li><strong>Centralized:</strong> Single point for all database operations</li>
                                    <li><strong>Simple:</strong> Direct database access without layering complexity</li>
                                    <li><strong>Reusable:</strong> Same DbCon class used throughout application</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-warning">
                            <h5 class="mb-0"><i class="fas fa-balance-scale me-2"></i>When to Use DbCon Pattern</h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6 class="text-success"><i class="fas fa-thumbs-up me-2"></i>Good For:</h6>
                                    <ul>
                                        <li>Small to medium applications</li>
                                        <li>Simple CRUD operations</li>
                                        <li>Rapid prototyping</li>
                                        <li>Internal tools and utilities</li>
                                        <li>Single developer projects</li>
                                        <li>Learning ADO.NET basics</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-danger"><i class="fas fa-exclamation-triangle me-2"></i>Not Ideal For:</h6>
                                    <ul>
                                        <li>Large enterprise applications</li>
                                        <li>Complex business logic</li>
                                        <li>Unit testing requirements</li>
                                        <li>Multiple presentation layers</li>
                                        <li>Team collaboration on layers</li>
                                        <li>Frequent business rule changes</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <footer class="bg-dark text-white text-center py-4 mt-5">
            <div class="container">
                <p class="mb-0">Centralized DbCon CRUD Application - ASP.NET Web Forms</p>
                <small>Simple, centralized database access pattern</small>
            </div>
        </footer>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
