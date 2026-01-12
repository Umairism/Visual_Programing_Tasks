<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MasterPage_Demo.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn about TechCorp Solutions - Our story, mission, and team" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item active" aria-current="page">About Us</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitlePlaceHolder" runat="server">
    <i class="fas fa-info-circle me-2"></i>About TechCorp Solutions
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Company Overview -->
    <div class="row mb-5">
        <div class="col-md-6 mb-4">
            <h2 class="mb-3">Our Story</h2>
            <p class="lead">Founded in 2020, TechCorp Solutions has been at the forefront of technological innovation.</p>
            <p>
                We started with a simple vision: to make technology accessible and beneficial for businesses of all sizes.
                Today, we serve over 1,250 clients worldwide, delivering cutting-edge solutions that drive growth and efficiency.
            </p>
            <p>
                Our team of dedicated professionals works tirelessly to understand your unique challenges and create
                customized solutions that exceed expectations. We believe in building long-term partnerships based on
                trust, quality, and innovation.
            </p>
        </div>
        
        <div class="col-md-6 mb-4">
            <div class="about-image-placeholder bg-light p-5 rounded text-center">
                <i class="fas fa-building fa-5x text-primary mb-3"></i>
                <p class="text-muted">Company Image Placeholder</p>
            </div>
        </div>
    </div>

    <!-- Mission, Vision, Values -->
    <div class="row mb-5">
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow border-primary">
                <div class="card-header bg-primary text-white text-center">
                    <i class="fas fa-bullseye fa-2x mb-2"></i>
                    <h5 class="mb-0">Our Mission</h5>
                </div>
                <div class="card-body">
                    <p>
                        To empower businesses through innovative technology solutions that drive efficiency,
                        growth, and digital transformation while maintaining the highest standards of quality and service.
                    </p>
                </div>
            </div>
        </div>
        
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow border-success">
                <div class="card-header bg-success text-white text-center">
                    <i class="fas fa-eye fa-2x mb-2"></i>
                    <h5 class="mb-0">Our Vision</h5>
                </div>
                <div class="card-body">
                    <p>
                        To be the world's most trusted technology partner, recognized for innovation, excellence,
                        and commitment to client success in every project we undertake.
                    </p>
                </div>
            </div>
        </div>
        
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow border-info">
                <div class="card-header bg-info text-white text-center">
                    <i class="fas fa-heart fa-2x mb-2"></i>
                    <h5 class="mb-0">Our Values</h5>
                </div>
                <div class="card-body">
                    <ul class="list-unstyled">
                        <li><i class="fas fa-check text-success me-2"></i>Innovation</li>
                        <li><i class="fas fa-check text-success me-2"></i>Integrity</li>
                        <li><i class="fas fa-check text-success me-2"></i>Excellence</li>
                        <li><i class="fas fa-check text-success me-2"></i>Customer Focus</li>
                        <li><i class="fas fa-check text-success me-2"></i>Collaboration</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <!-- Team Section -->
    <div class="row mb-5">
        <div class="col-12 mb-4">
            <h2 class="text-center">
                <i class="fas fa-users text-primary me-2"></i>Meet Our Team
            </h2>
            <p class="text-center text-muted">The talented people behind TechCorp Solutions</p>
        </div>
        
        <asp:Repeater ID="rptTeam" runat="server">
            <ItemTemplate>
                <div class="col-md-3 col-sm-6 mb-4">
                    <div class="card team-card shadow-sm h-100">
                        <div class="card-body text-center">
                            <div class="team-avatar mb-3">
                                <i class="fas fa-user-circle fa-5x text-primary"></i>
                            </div>
                            <h5 class="card-title"><%# Eval("Name") %></h5>
                            <p class="text-muted small mb-2"><%# Eval("Position") %></p>
                            <p class="small"><%# Eval("Description") %></p>
                            <div class="team-social mt-3">
                                <a href="#" class="text-primary me-2"><i class="fab fa-linkedin"></i></a>
                                <a href="#" class="text-info me-2"><i class="fab fa-twitter"></i></a>
                                <a href="#" class="text-danger"><i class="fas fa-envelope"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Timeline -->
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="text-center mb-4">
                <i class="fas fa-history text-primary me-2"></i>Our Journey
            </h2>
        </div>
        
        <div class="col-12">
            <div class="timeline">
                <div class="timeline-item mb-4">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <h5 class="card-title text-primary">2020 - Foundation</h5>
                                    <p class="card-text">
                                        TechCorp Solutions was founded with a vision to revolutionize technology services.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="timeline-item mb-4">
                    <div class="row">
                        <div class="col-md-6 offset-md-6">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <h5 class="card-title text-success">2022 - Expansion</h5>
                                    <p class="card-text">
                                        Expanded operations to serve clients in 15+ countries with 500+ completed projects.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="timeline-item mb-4">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <h5 class="card-title text-info">2024 - Recognition</h5>
                                    <p class="card-text">
                                        Received multiple industry awards for innovation and customer satisfaction.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="timeline-item mb-4">
                    <div class="row">
                        <div class="col-md-6 offset-md-6">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <h5 class="card-title text-warning">2026 - Innovation</h5>
                                    <p class="card-text">
                                        Launched groundbreaking AI-powered solutions and expanded our service portfolio.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SidebarContent" runat="server">
    <div class="card shadow-sm mb-3">
        <div class="card-header bg-primary text-white">
            <i class="fas fa-award"></i> Achievements
        </div>
        <div class="card-body">
            <ul class="list-unstyled">
                <li class="mb-2">
                    <i class="fas fa-trophy text-warning me-2"></i>
                    Best Innovation 2025
                </li>
                <li class="mb-2">
                    <i class="fas fa-trophy text-warning me-2"></i>
                    Top Tech Company 2024
                </li>
                <li class="mb-2">
                    <i class="fas fa-trophy text-warning me-2"></i>
                    Customer Choice Award
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
