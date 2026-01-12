<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ADO_CRUD.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pure ADO.NET CRUD</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="Styles/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container">
                <a class="navbar-brand" href="Default.aspx">
                    <i class="fas fa-code me-2"></i>Pure ADO.NET CRUD
                </a>
                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" href="Default.aspx"><i class="fas fa-home me-1"></i>Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Students/StudentList.aspx"><i class="fas fa-user-graduate me-1"></i>Students</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="Courses/CourseList.aspx"><i class="fas fa-book me-1"></i>Courses</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="hero-section bg-primary text-white py-5">
            <div class="container text-center">
                <h1 class="display-4 mb-3">
                    <i class="fas fa-database me-3"></i>Pure ADO.NET Approach
                </h1>
                <p class="lead">Student Management System using Direct Database Access</p>
                <p class="mt-4">No utility classes • No layers • Just raw ADO.NET code</p>
            </div>
        </div>

        <div class="container mt-5">
            <!-- Statistics Cards -->
            <div class="row mb-4">
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-user-graduate fa-2x text-primary mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblTotalStudents" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Total Students</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-book fa-2x text-info mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblTotalCourses" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Total Courses</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-chart-line fa-2x text-success mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblAverageGPA" runat="server" Text="0.00" /></h3>
                            <p class="text-muted mb-0">Average GPA</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="card stat-card shadow">
                        <div class="card-body text-center">
                            <i class="fas fa-star fa-2x text-warning mb-2"></i>
                            <h3 class="mb-0"><asp:Label ID="lblExcellent" runat="server" Text="0" /></h3>
                            <p class="text-muted mb-0">Excellent (GPA ≥ 3.75)</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 offset-md-3 mb-4">
                    <div class="card shadow architecture-card">
                        <div class="card-body text-center">
                            <div class="architecture-icon bg-primary text-white mb-3">
                                <i class="fas fa-layer-group fa-3x"></i>
                            </div>
                            <h5 class="card-title">Pure ADO.NET Architecture</h5>
                            <div class="architecture-flow">
                                <div class="flow-box bg-light">
                                    <i class="fas fa-desktop"></i>
                                    <p class="mb-0"><strong>ASPX Pages</strong></p>
                                    <small>User Interface</small>
                                </div>
                                <div class="flow-arrow">↓ directly uses</div>
                                <div class="flow-box bg-primary text-white">
                                    <i class="fas fa-code"></i>
                                    <p class="mb-0"><strong>ADO.NET Code</strong></p>
                                    <small>SqlConnection, SqlCommand</small>
                                </div>
                                <div class="flow-arrow">↓ queries</div>
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
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0"><i class="fas fa-user-graduate me-2"></i>Student Management</h5>
                        </div>
                        <div class="card-body">
                            <p>Manage student records with direct ADO.NET operations:</p>
                            <ul>
                                <li>View all students with course details</li>
                                <li>Add new students</li>
                                <li>Update student information</li>
                                <li>Delete student records</li>
                                <li>Search and filter students</li>
                                <li>Track GPA and performance</li>
                            </ul>
                            <a href="Students/StudentList.aspx" class="btn btn-primary">
                                <i class="fas fa-arrow-right me-2"></i>Manage Students
                            </a>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 mb-4">
                    <div class="card shadow h-100">
                        <div class="card-header bg-info text-white">
                            <h5 class="mb-0"><i class="fas fa-book me-2"></i>Course Management</h5>
                        </div>
                        <div class="card-body">
                            <p>Manage courses using ADO.NET:</p>
                            <ul>
                                <li>View all available courses</li>
                                <li>See course details and credits</li>
                                <li>Track student enrollment</li>
                                <li>Filter by department</li>
                                <li>Manage course status</li>
                            </ul>
                            <a href="Courses/CourseList.aspx" class="btn btn-info">
                                <i class="fas fa-arrow-right me-2"></i>View Courses
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card shadow">
                        <div class="card-header bg-dark text-white">
                            <h5 class="mb-0"><i class="fas fa-code me-2"></i>ADO.NET Classes Used</h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6 class="text-primary"><i class="fas fa-check-circle me-2"></i>Core Classes</h6>
                                    <ul class="list-unstyled">
                                        <li><code>SqlConnection</code> - Database connection</li>
                                        <li><code>SqlCommand</code> - Execute SQL commands</li>
                                        <li><code>SqlDataReader</code> - Forward-only data reading</li>
                                        <li><code>SqlDataAdapter</code> - Fill DataTable/DataSet</li>
                                        <li><code>SqlParameter</code> - Parameterized queries</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-primary"><i class="fas fa-check-circle me-2"></i>Data Containers</h6>
                                    <ul class="list-unstyled">
                                        <li><code>DataTable</code> - In-memory table</li>
                                        <li><code>DataSet</code> - Multiple tables</li>
                                        <li><code>DataRow</code> - Single row of data</li>
                                        <li><code>CommandType</code> - Text or StoredProcedure</li>
                                        <li><code>CommandBehavior</code> - Reader options</li>
                                    </ul>
                                </div>
                            </div>

                            <div class="alert alert-info mt-3">
                                <h6><i class="fas fa-info-circle me-2"></i>Key Characteristics</h6>
                                <ul class="mb-0">
                                    <li><strong>Direct Access:</strong> Code-behind files directly use ADO.NET classes</li>
                                    <li><strong>Manual Management:</strong> Explicit connection opening/closing</li>
                                    <li><strong>No Abstraction:</strong> No utility classes or helper methods</li>
                                    <li><strong>Basic Pattern:</strong> Shows fundamental database operations</li>
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
                            <h5 class="mb-0"><i class="fas fa-lightbulb me-2"></i>When to Use Pure ADO.NET</h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <h6 class="text-success"><i class="fas fa-thumbs-up me-2"></i>Good For:</h6>
                                    <ul>
                                        <li>Learning ADO.NET fundamentals</li>
                                        <li>Understanding database operations</li>
                                        <li>Very simple applications</li>
                                        <li>Prototypes and demos</li>
                                        <li>Full control over SQL</li>
                                        <li>Performance-critical operations</li>
                                    </ul>
                                </div>
                                <div class="col-md-6">
                                    <h6 class="text-danger"><i class="fas fa-exclamation-triangle me-2"></i>Not Recommended For:</h6>
                                    <ul>
                                        <li>Production applications</li>
                                        <li>Large codebases</li>
                                        <li>Team projects</li>
                                        <li>Complex business logic</li>
                                        <li>Applications requiring maintainability</li>
                                        <li>Projects with tight deadlines</li>
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
                <p class="mb-0">Pure ADO.NET CRUD Application - ASP.NET Web Forms</p>
                <small>Direct database access without abstraction layers</small>
            </div>
        </footer>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
