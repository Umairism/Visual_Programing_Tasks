<%@ Page Title="Services" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="MasterPage_Demo.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="TechCorp Services - Consulting, development, support, and more" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item active" aria-current="page">Services</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitlePlaceHolder" runat="server">
    <i class="fas fa-cogs me-2"></i>Our Services
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Services Overview -->
    <div class="row mb-5">
        <div class="col-12">
            <div class="jumbotron bg-light p-4 rounded">
                <h2>Professional Services Tailored to Your Needs</h2>
                <p class="lead">
                    From consulting to implementation, we provide comprehensive services to help your business succeed in the digital age.
                </p>
            </div>
        </div>
    </div>

    <!-- Main Services -->
    <div class="row mb-5">
        <div class="col-12 mb-4">
            <h3 class="text-center">
                <i class="fas fa-star text-warning me-2"></i>Our Core Services
            </h3>
        </div>

        <!-- Service 1: Consulting -->
        <div class="col-md-6 mb-4">
            <div class="card h-100 shadow-sm service-card">
                <div class="card-body">
                    <div class="service-icon mb-3 text-center">
                        <i class="fas fa-chart-line fa-4x text-primary"></i>
                    </div>
                    <h4 class="card-title text-center">Technology Consulting</h4>
                    <p class="card-text">
                        Expert guidance to help you make informed technology decisions and develop effective digital strategies.
                    </p>
                    <ul class="list-unstyled">
                        <li><i class="fas fa-check-circle text-success me-2"></i>Digital transformation roadmap</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Technology stack evaluation</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Process optimization</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Strategic planning</li>
                    </ul>
                    <div class="text-center mt-3">
                        <span class="h5 text-primary">Starting at $150/hour</span>
                    </div>
                </div>
                <div class="card-footer bg-white text-center">
                    <a href="Contact.aspx" class="btn btn-primary">Request Consultation</a>
                </div>
            </div>
        </div>

        <!-- Service 2: Development -->
        <div class="col-md-6 mb-4">
            <div class="card h-100 shadow-sm service-card">
                <div class="card-body">
                    <div class="service-icon mb-3 text-center">
                        <i class="fas fa-code fa-4x text-success"></i>
                    </div>
                    <h4 class="card-title text-center">Custom Development</h4>
                    <p class="card-text">
                        Build tailored software solutions that perfectly match your business requirements and workflows.
                    </p>
                    <ul class="list-unstyled">
                        <li><i class="fas fa-check-circle text-success me-2"></i>Web application development</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Mobile app development</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>API development & integration</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Database design</li>
                    </ul>
                    <div class="text-center mt-3">
                        <span class="h5 text-success">Custom Quotes</span>
                    </div>
                </div>
                <div class="card-footer bg-white text-center">
                    <a href="Contact.aspx" class="btn btn-success">Start Your Project</a>
                </div>
            </div>
        </div>

        <!-- Service 3: Cloud Migration -->
        <div class="col-md-6 mb-4">
            <div class="card h-100 shadow-sm service-card">
                <div class="card-body">
                    <div class="service-icon mb-3 text-center">
                        <i class="fas fa-cloud-upload-alt fa-4x text-info"></i>
                    </div>
                    <h4 class="card-title text-center">Cloud Migration</h4>
                    <p class="card-text">
                        Seamlessly move your infrastructure to the cloud with minimal downtime and maximum efficiency.
                    </p>
                    <ul class="list-unstyled">
                        <li><i class="fas fa-check-circle text-success me-2"></i>Cloud readiness assessment</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Migration planning</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Data transfer & testing</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Post-migration support</li>
                    </ul>
                    <div class="text-center mt-3">
                        <span class="h5 text-info">Starting at $5,000</span>
                    </div>
                </div>
                <div class="card-footer bg-white text-center">
                    <a href="Contact.aspx" class="btn btn-info">Plan Migration</a>
                </div>
            </div>
        </div>

        <!-- Service 4: Support -->
        <div class="col-md-6 mb-4">
            <div class="card h-100 shadow-sm service-card">
                <div class="card-body">
                    <div class="service-icon mb-3 text-center">
                        <i class="fas fa-headset fa-4x text-warning"></i>
                    </div>
                    <h4 class="card-title text-center">24/7 Technical Support</h4>
                    <p class="card-text">
                        Round-the-clock technical support to keep your systems running smoothly without interruption.
                    </p>
                    <ul class="list-unstyled">
                        <li><i class="fas fa-check-circle text-success me-2"></i>24/7 availability</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Response within 1 hour</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>Remote troubleshooting</li>
                        <li><i class="fas fa-check-circle text-success me-2"></i>System monitoring</li>
                    </ul>
                    <div class="text-center mt-3">
                        <span class="h5 text-warning">$500/month</span>
                    </div>
                </div>
                <div class="card-footer bg-white text-center">
                    <a href="Contact.aspx" class="btn btn-warning">Get Support</a>
                </div>
            </div>
        </div>
    </div>

    <!-- Process Section -->
    <div class="row mb-5">
        <div class="col-12 mb-4">
            <h3 class="text-center">
                <i class="fas fa-tasks text-primary me-2"></i>Our Process
            </h3>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="text-center p-4 bg-light rounded">
                <div class="process-number bg-primary text-white rounded-circle d-inline-flex align-items-center justify-content-center mb-3"
                     style="width: 60px; height: 60px;">
                    <h3 class="mb-0">1</h3>
                </div>
                <h5>Discovery</h5>
                <p class="small text-muted">Understanding your needs and goals</p>
            </div>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="text-center p-4 bg-light rounded">
                <div class="process-number bg-success text-white rounded-circle d-inline-flex align-items-center justify-content-center mb-3"
                     style="width: 60px; height: 60px;">
                    <h3 class="mb-0">2</h3>
                </div>
                <h5>Planning</h5>
                <p class="small text-muted">Creating a detailed project roadmap</p>
            </div>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="text-center p-4 bg-light rounded">
                <div class="process-number bg-info text-white rounded-circle d-inline-flex align-items-center justify-content-center mb-3"
                     style="width: 60px; height: 60px;">
                    <h3 class="mb-0">3</h3>
                </div>
                <h5>Execution</h5>
                <p class="small text-muted">Implementing the solution</p>
            </div>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="text-center p-4 bg-light rounded">
                <div class="process-number bg-warning text-white rounded-circle d-inline-flex align-items-center justify-content-center mb-3"
                     style="width: 60px; height: 60px;">
                    <h3 class="mb-0">4</h3>
                </div>
                <h5>Support</h5>
                <p class="small text-muted">Ongoing maintenance and optimization</p>
            </div>
        </div>
    </div>

    <!-- Testimonials -->
    <div class="row mb-4">
        <div class="col-12 mb-4">
            <h3 class="text-center">
                <i class="fas fa-comments text-primary me-2"></i>What Our Clients Say
            </h3>
        </div>

        <div class="col-md-4 mb-3">
            <div class="card shadow-sm h-100">
                <div class="card-body">
                    <div class="mb-3">
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                    </div>
                    <p class="card-text">
                        "TechCorp's consulting services helped us transform our business. Their expertise is unmatched!"
                    </p>
                    <hr/>
                    <p class="mb-0"><strong>Jane Smith</strong></p>
                    <p class="small text-muted">CEO, Tech Innovations Inc.</p>
                </div>
            </div>
        </div>

        <div class="col-md-4 mb-3">
            <div class="card shadow-sm h-100">
                <div class="card-body">
                    <div class="mb-3">
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                    </div>
                    <p class="card-text">
                        "The development team delivered our project on time and exceeded our expectations!"
                    </p>
                    <hr/>
                    <p class="mb-0"><strong>Robert Johnson</strong></p>
                    <p class="small text-muted">CTO, Digital Solutions Ltd.</p>
                </div>
            </div>
        </div>

        <div class="col-md-4 mb-3">
            <div class="card shadow-sm h-100">
                <div class="card-body">
                    <div class="mb-3">
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                        <i class="fas fa-star text-warning"></i>
                    </div>
                    <p class="card-text">
                        "Their 24/7 support has been a lifesaver for our operations. Highly recommended!"
                    </p>
                    <hr/>
                    <p class="mb-0"><strong>Maria Garcia</strong></p>
                    <p class="small text-muted">Operations Manager, Global Corp</p>
                </div>
            </div>
        </div>
    </div>

    <!-- CTA -->
    <div class="row">
        <div class="col-12">
            <div class="card bg-primary text-white shadow">
                <div class="card-body text-center p-5">
                    <h3><i class="fas fa-rocket me-2"></i>Ready to Get Started?</h3>
                    <p class="lead mb-4">
                        Let's discuss how our services can help your business grow and succeed.
                    </p>
                    <a href="Contact.aspx" class="btn btn-light btn-lg px-5">
                        <i class="fas fa-phone me-2"></i>Schedule a Free Consultation
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SidebarContent" runat="server">
    <div class="card shadow-sm mb-3">
        <div class="card-header bg-primary text-white">
            <i class="fas fa-tags"></i> Special Offers
        </div>
        <div class="card-body">
            <div class="alert alert-success mb-2">
                <strong>20% OFF</strong><br/>
                <small>First-time customers</small>
            </div>
            <div class="alert alert-info mb-0">
                <strong>Free Consultation</strong><br/>
                <small>Book your free 1-hour session</small>
            </div>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-header bg-success text-white">
            <i class="fas fa-file-download"></i> Downloads
        </div>
        <div class="card-body">
            <ul class="list-unstyled">
                <li class="mb-2">
                    <a href="#" class="text-decoration-none">
                        <i class="fas fa-file-pdf text-danger me-2"></i>Service Catalog
                    </a>
                </li>
                <li class="mb-2">
                    <a href="#" class="text-decoration-none">
                        <i class="fas fa-file-pdf text-danger me-2"></i>Case Studies
                    </a>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
