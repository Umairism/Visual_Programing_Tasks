<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MasterPage_Demo.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Contact TechCorp Solutions - Get in touch with us" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item active" aria-current="page">Contact</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitlePlaceHolder" runat="server">
    <i class="fas fa-envelope me-2"></i>Contact Us
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Contact Form -->
        <div class="col-md-8 mb-4">
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-paper-plane me-2"></i>Send Us a Message</h5>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                        <asp:Label ID="lblMessage" runat="server" />
                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                    </asp:Panel>
                    
                    <div class="mb-3">
                        <label for="txtName" class="form-label">
                            <i class="fas fa-user me-1"></i>Full Name *
                        </label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Name is required" Display="Dynamic" CssClass="text-danger small" />
                    </div>
                    
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">
                            <i class="fas fa-envelope me-1"></i>Email Address *
                        </label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="your.email@example.com" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Email is required" Display="Dynamic" CssClass="text-danger small" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                            ErrorMessage="Please enter a valid email address" Display="Dynamic" CssClass="text-danger small" />
                    </div>
                    
                    <div class="mb-3">
                        <label for="txtPhone" class="form-label">
                            <i class="fas fa-phone me-1"></i>Phone Number
                        </label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+1 (555) 123-4567" />
                    </div>
                    
                    <div class="mb-3">
                        <label for="ddlSubject" class="form-label">
                            <i class="fas fa-tag me-1"></i>Subject *
                        </label>
                        <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">-- Select Subject --</asp:ListItem>
                            <asp:ListItem Value="General Inquiry">General Inquiry</asp:ListItem>
                            <asp:ListItem Value="Product Support">Product Support</asp:ListItem>
                            <asp:ListItem Value="Sales">Sales</asp:ListItem>
                            <asp:ListItem Value="Technical Support">Technical Support</asp:ListItem>
                            <asp:ListItem Value="Partnership">Partnership Opportunity</asp:ListItem>
                            <asp:ListItem Value="Feedback">Feedback</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="ddlSubject"
                            InitialValue="" ErrorMessage="Please select a subject" Display="Dynamic" CssClass="text-danger small" />
                    </div>
                    
                    <div class="mb-3">
                        <label for="txtMessage" class="form-label">
                            <i class="fas fa-comment me-1"></i>Message *
                        </label>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="5" placeholder="Type your message here..." />
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                            ErrorMessage="Message is required" Display="Dynamic" CssClass="text-danger small" />
                    </div>
                    
                    <div class="mb-3">
                        <asp:CheckBox ID="chkNewsletter" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label ms-2" for="chkNewsletter">
                            Subscribe to our newsletter for updates and offers
                        </label>
                    </div>
                    
                    <div class="d-grid gap-2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Send Message" CssClass="btn btn-primary btn-lg"
                            OnClick="btnSubmit_Click">
                            <i class="fas fa-paper-plane me-2"></i>
                        </asp:Button>
                        <asp:Button ID="btnReset" runat="server" Text="Reset Form" CssClass="btn btn-secondary"
                            OnClick="btnReset_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Contact Information -->
        <div class="col-md-4">
            <div class="card shadow mb-3">
                <div class="card-header bg-success text-white">
                    <h5 class="mb-0"><i class="fas fa-map-marker-alt me-2"></i>Our Office</h5>
                </div>
                <div class="card-body">
                    <p>
                        <strong>TechCorp Solutions</strong><br/>
                        123 Tech Street<br/>
                        Silicon Valley, CA 94025<br/>
                        United States
                    </p>
                    <hr/>
                    <p>
                        <i class="fas fa-phone text-primary me-2"></i>
                        <strong>Phone:</strong><br/>
                        +1 (555) 123-4567
                    </p>
                    <p>
                        <i class="fas fa-envelope text-primary me-2"></i>
                        <strong>Email:</strong><br/>
                        info@techcorp.com
                    </p>
                    <p>
                        <i class="fas fa-clock text-primary me-2"></i>
                        <strong>Business Hours:</strong><br/>
                        Monday - Friday: 9:00 AM - 6:00 PM<br/>
                        Saturday: 10:00 AM - 4:00 PM<br/>
                        Sunday: Closed
                    </p>
                </div>
            </div>
            
            <div class="card shadow mb-3">
                <div class="card-header bg-info text-white">
                    <h5 class="mb-0"><i class="fas fa-globe me-2"></i>Follow Us</h5>
                </div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <a href="#" class="btn btn-outline-primary">
                            <i class="fab fa-facebook me-2"></i>Facebook
                        </a>
                        <a href="#" class="btn btn-outline-info">
                            <i class="fab fa-twitter me-2"></i>Twitter
                        </a>
                        <a href="#" class="btn btn-outline-primary">
                            <i class="fab fa-linkedin me-2"></i>LinkedIn
                        </a>
                        <a href="#" class="btn btn-outline-dark">
                            <i class="fab fa-github me-2"></i>GitHub
                        </a>
                    </div>
                </div>
            </div>
            
            <div class="card shadow">
                <div class="card-header bg-warning text-dark">
                    <h5 class="mb-0"><i class="fas fa-headset me-2"></i>24/7 Support</h5>
                </div>
                <div class="card-body">
                    <p>Need immediate assistance? Our support team is available 24/7.</p>
                    <a href="#" class="btn btn-warning w-100">
                        <i class="fas fa-comments me-2"></i>Live Chat
                    </a>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Map Section -->
    <div class="row mt-4">
        <div class="col-12">
            <div class="card shadow">
                <div class="card-header bg-secondary text-white">
                    <h5 class="mb-0"><i class="fas fa-map me-2"></i>Find Us on Map</h5>
                </div>
                <div class="card-body p-0">
                    <div class="map-placeholder bg-light p-5 text-center">
                        <i class="fas fa-map-marked-alt fa-5x text-muted mb-3"></i>
                        <p class="text-muted">Interactive Map Placeholder</p>
                        <p class="small text-muted">123 Tech Street, Silicon Valley, CA 94025</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SidebarContent" runat="server">
    <div class="card shadow-sm">
        <div class="card-header bg-primary text-white">
            <i class="fas fa-question-circle"></i> FAQ
        </div>
        <div class="card-body">
            <p class="small"><strong>Q: How quickly do you respond?</strong></p>
            <p class="small">A: We respond to all inquiries within 24 hours.</p>
            <hr/>
            <p class="small"><strong>Q: Do you offer phone support?</strong></p>
            <p class="small">A: Yes, call us at +1 (555) 123-4567.</p>
        </div>
    </div>
</asp:Content>
