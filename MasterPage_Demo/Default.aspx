<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasterPage_Demo.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Additional head content for this page -->
    <meta name="description" content="TechCorp Solutions - Innovation Through Technology" />
    <meta name="keywords" content="technology, solutions, innovation" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item active" aria-current="page">Home</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitlePlaceHolder" runat="server">
    <i class="fas fa-home me-2"></i>Welcome to TechCorp Solutions
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Section -->
    <div class="hero-section text-center mb-5">
        <div class="jumbotron p-5 rounded-3 bg-light">
            <h1 class="display-4">Innovation Through Technology</h1>
            <p class="lead">Empowering businesses with cutting-edge solutions</p>
            <hr class="my-4" />
            <p>We deliver high-quality products and services that transform your digital experience.</p>
            <div class="btn-group" role="group">
                <a href="Products.aspx" class="btn btn-primary btn-lg px-4 me-2">
                    <i class="fas fa-box me-2"></i>View Products
                </a>
                <a href="Services.aspx" class="btn btn-success btn-lg px-4">
                    <i class="fas fa-cogs me-2"></i>Our Services
                </a>
            </div>
        </div>
    </div>

    <!-- Features Section -->
    <div class="row mb-5">
        <div class="col-12">
            <h2 class="text-center mb-4">
                <i class="fas fa-star text-warning me-2"></i>Why Choose Us?
            </h2>
        </div>
        
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow-sm hover-card">
                <div class="card-body text-center">
                    <div class="feature-icon mb-3">
                        <i class="fas fa-rocket fa-3x text-primary"></i>
                    </div>
                    <h5 class="card-title">Fast & Reliable</h5>
                    <p class="card-text">
                        Lightning-fast performance and 99.9% uptime guarantee ensures your business never stops.
                    </p>
                </div>
            </div>
        </div>
        
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow-sm hover-card">
                <div class="card-body text-center">
                    <div class="feature-icon mb-3">
                        <i class="fas fa-shield-alt fa-3x text-success"></i>
                    </div>
                    <h5 class="card-title">Secure & Protected</h5>
                    <p class="card-text">
                        Enterprise-grade security with encryption, authentication, and regular security audits.
                    </p>
                </div>
            </div>
        </div>
        
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow-sm hover-card">
                <div class="card-body text-center">
                    <div class="feature-icon mb-3">
                        <i class="fas fa-users fa-3x text-info"></i>
                    </div>
                    <h5 class="card-title">Expert Support</h5>
                    <p class="card-text">
                        24/7 customer support from our team of experienced professionals ready to help.
                    </p>
                </div>
            </div>
        </div>
    </div>

    <!-- Statistics Section -->
    <div class="row mb-5 text-center">
        <div class="col-md-3 col-sm-6 mb-4">
            <div class="stat-card p-4 rounded bg-primary text-white">
                <i class="fas fa-users fa-2x mb-3"></i>
                <h3 class="display-6">
                    <asp:Label ID="lblCustomers" runat="server" Text="1,250+" />
                </h3>
                <p class="mb-0">Happy Customers</p>
            </div>
        </div>
        
        <div class="col-md-3 col-sm-6 mb-4">
            <div class="stat-card p-4 rounded bg-success text-white">
                <i class="fas fa-project-diagram fa-2x mb-3"></i>
                <h3 class="display-6">
                    <asp:Label ID="lblProjects" runat="server" Text="2,500+" />
                </h3>
                <p class="mb-0">Projects Completed</p>
            </div>
        </div>
        
        <div class="col-md-3 col-sm-6 mb-4">
            <div class="stat-card p-4 rounded bg-info text-white">
                <i class="fas fa-award fa-2x mb-3"></i>
                <h3 class="display-6">
                    <asp:Label ID="lblAwards" runat="server" Text="50+" />
                </h3>
                <p class="mb-0">Awards Won</p>
            </div>
        </div>
        
        <div class="col-md-3 col-sm-6 mb-4">
            <div class="stat-card p-4 rounded bg-warning text-white">
                <i class="fas fa-clock fa-2x mb-3"></i>
                <h3 class="display-6">
                    <asp:Label ID="lblYears" runat="server" Text="6+" />
                </h3>
                <p class="mb-0">Years in Business</p>
            </div>
        </div>
    </div>

    <!-- Latest News Section -->
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-center mb-4">
                <i class="fas fa-newspaper text-primary me-2"></i>Latest News & Updates
            </h2>
        </div>
        
        <div class="col-md-6 mb-3">
            <div class="card shadow-sm">
                <div class="card-body">
                    <span class="badge bg-primary mb-2">Product Launch</span>
                    <h5 class="card-title">New Cloud Platform Released</h5>
                    <p class="card-text text-muted small">
                        <i class="far fa-calendar me-2"></i>January 10, 2026
                    </p>
                    <p class="card-text">
                        We're excited to announce the launch of our new cloud platform with enhanced features...
                    </p>
                    <a href="#" class="btn btn-sm btn-outline-primary">Read More</a>
                </div>
            </div>
        </div>
        
        <div class="col-md-6 mb-3">
            <div class="card shadow-sm">
                <div class="card-body">
                    <span class="badge bg-success mb-2">Achievement</span>
                    <h5 class="card-title">TechCorp Wins Innovation Award</h5>
                    <p class="card-text text-muted small">
                        <i class="far fa-calendar me-2"></i>January 5, 2026
                    </p>
                    <p class="card-text">
                        Recognized for excellence in technology innovation and customer satisfaction...
                    </p>
                    <a href="#" class="btn btn-sm btn-outline-success">Read More</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Call to Action -->
    <div class="row">
        <div class="col-12">
            <div class="cta-section p-5 text-center bg-gradient-primary text-white rounded">
                <h2 class="mb-3">Ready to Get Started?</h2>
                <p class="lead mb-4">Join thousands of satisfied customers and transform your business today!</p>
                <a href="Contact.aspx" class="btn btn-light btn-lg px-5">
                    <i class="fas fa-envelope me-2"></i>Contact Us Now
                </a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SidebarContent" runat="server">
    <!-- Custom Sidebar for Home Page -->
    <div class="card shadow-sm mb-3">
        <div class="card-header bg-primary text-white">
            <i class="fas fa-bullhorn"></i> Latest Updates
        </div>
        <div class="card-body">
            <ul class="list-unstyled">
                <li class="mb-2">
                    <i class="fas fa-check-circle text-success me-2"></i>
                    New products available
                </li>
                <li class="mb-2">
                    <i class="fas fa-check-circle text-success me-2"></i>
                    24/7 support launched
                </li>
                <li class="mb-2">
                    <i class="fas fa-check-circle text-success me-2"></i>
                    Enhanced security features
                </li>
            </ul>
        </div>
    </div>
    
    <div class="card shadow-sm">
        <div class="card-header bg-success text-white">
            <i class="fas fa-phone"></i> Need Help?
        </div>
        <div class="card-body">
            <p class="mb-2"><strong>Call us:</strong></p>
            <p class="h5 text-primary">+1 (555) 123-4567</p>
            <p class="small text-muted mt-3">Available Monday - Friday<br/>9:00 AM - 6:00 PM EST</p>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script>
        // Custom JavaScript for home page
        document.addEventListener('DOMContentLoaded', function() {
            console.log('Home page loaded successfully!');
            
            // Add hover effect to feature cards
            const cards = document.querySelectorAll('.hover-card');
            cards.forEach(card => {
                card.addEventListener('mouseenter', function() {
                    this.style.transform = 'translateY(-10px)';
                    this.style.transition = 'transform 0.3s ease';
                });
                
                card.addEventListener('mouseleave', function() {
                    this.style.transform = 'translateY(0)';
                });
            });
        });
    </script>
</asp:Content>
