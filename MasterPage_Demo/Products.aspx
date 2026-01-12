<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MasterPage_Demo.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="TechCorp Products - Cloud solutions, software, and more" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
    <li class="breadcrumb-item active" aria-current="page">Products</li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitlePlaceHolder" runat="server">
    <i class="fas fa-box me-2"></i>Our Products
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Products Introduction -->
    <div class="alert alert-info mb-4">
        <i class="fas fa-info-circle me-2"></i>
        <strong>Explore our innovative products</strong> designed to transform your business operations and drive growth.
    </div>

    <!-- Product Categories -->
    <div class="row mb-4">
        <div class="col-12">
            <h3 class="mb-3">Product Categories</h3>
            <div class="btn-group mb-4" role="group">
                <asp:Button ID="btnAll" runat="server" Text="All Products" CssClass="btn btn-outline-primary active" 
                    OnClick="btnFilter_Click" CommandArgument="All" />
                <asp:Button ID="btnCloud" runat="server" Text="Cloud Solutions" CssClass="btn btn-outline-primary" 
                    OnClick="btnFilter_Click" CommandArgument="Cloud" />
                <asp:Button ID="btnSoftware" runat="server" Text="Software" CssClass="btn btn-outline-primary" 
                    OnClick="btnFilter_Click" CommandArgument="Software" />
                <asp:Button ID="btnSecurity" runat="server" Text="Security" CssClass="btn btn-outline-primary" 
                    OnClick="btnFilter_Click" CommandArgument="Security" />
            </div>
        </div>
    </div>

    <!-- Products Grid -->
    <div class="row">
        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <ItemTemplate>
                <div class="col-md-6 col-lg-4 mb-4">
                    <div class="card h-100 shadow-sm product-card">
                        <div class="card-header <%# GetCategoryColor(Eval("Category").ToString()) %> text-white">
                            <h5 class="mb-0">
                                <i class="<%# GetCategoryIcon(Eval("Category").ToString()) %> me-2"></i>
                                <%# Eval("Name") %>
                            </h5>
                        </div>
                        <div class="card-body">
                            <span class="badge bg-secondary mb-2"><%# Eval("Category") %></span>
                            <p class="card-text"><%# Eval("Description") %></p>
                            
                            <h6 class="text-muted">Key Features:</h6>
                            <ul class="small">
                                <%# GetFeaturesList(Eval("Features").ToString()) %>
                            </ul>
                            
                            <div class="d-flex justify-content-between align-items-center mt-3">
                                <span class="h4 mb-0 text-primary"><%# Eval("Price") %></span>
                                <span class="badge <%# GetPopularityBadge(Eval("Popularity").ToString()) %>">
                                    <%# Eval("Popularity") %>
                                </span>
                            </div>
                        </div>
                        <div class="card-footer bg-white">
                            <asp:Button ID="btnViewDetails" runat="server" Text="View Details" 
                                CssClass="btn btn-primary w-100" CommandName="ViewDetails" 
                                CommandArgument='<%# Eval("ProductId") %>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- No Products Message -->
    <asp:Panel ID="pnlNoProducts" runat="server" Visible="false" CssClass="alert alert-warning text-center">
        <i class="fas fa-exclamation-triangle fa-2x mb-3"></i>
        <p class="mb-0">No products found in this category.</p>
    </asp:Panel>

    <!-- Product Details Modal Placeholder -->
    <asp:Panel ID="pnlProductDetails" runat="server" Visible="false" CssClass="modal-overlay">
        <div class="modal-content-custom">
            <div class="card shadow-lg">
                <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">
                        <i class="fas fa-info-circle me-2"></i>Product Details
                    </h5>
                    <asp:Button ID="btnCloseModal" runat="server" Text="×" CssClass="btn btn-close btn-close-white" 
                        OnClick="btnCloseModal_Click" />
                </div>
                <div class="card-body">
                    <h4><asp:Label ID="lblProductName" runat="server" /></h4>
                    <p class="text-muted"><asp:Label ID="lblProductCategory" runat="server" /></p>
                    <hr/>
                    <p><asp:Label ID="lblProductDescription" runat="server" /></p>
                    <h5 class="mt-4">Pricing</h5>
                    <p class="h3 text-primary"><asp:Label ID="lblProductPrice" runat="server" /></p>
                    <div class="mt-4">
                        <a href="Contact.aspx" class="btn btn-success btn-lg">
                            <i class="fas fa-shopping-cart me-2"></i>Request Quote
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Call to Action -->
    <div class="row mt-5">
        <div class="col-12">
            <div class="card bg-gradient-primary text-white shadow">
                <div class="card-body text-center p-5">
                    <h3><i class="fas fa-lightbulb me-2"></i>Custom Solutions Available</h3>
                    <p class="lead mb-4">
                        Don't see what you need? We can build custom solutions tailored to your specific requirements.
                    </p>
                    <a href="Contact.aspx" class="btn btn-light btn-lg px-5">
                        <i class="fas fa-envelope me-2"></i>Contact Sales Team
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SidebarContent" runat="server">
    <div class="card shadow-sm mb-3">
        <div class="card-header bg-primary text-white">
            <i class="fas fa-fire"></i> Popular Products
        </div>
        <div class="card-body">
            <ul class="list-unstyled">
                <li class="mb-2">
                    <i class="fas fa-star text-warning me-2"></i>
                    <a href="#" class="text-decoration-none">CloudHub Pro</a>
                </li>
                <li class="mb-2">
                    <i class="fas fa-star text-warning me-2"></i>
                    <a href="#" class="text-decoration-none">SecureShield Plus</a>
                </li>
                <li class="mb-2">
                    <i class="fas fa-star text-warning me-2"></i>
                    <a href="#" class="text-decoration-none">DataMaster Suite</a>
                </li>
            </ul>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-header bg-success text-white">
            <i class="fas fa-download"></i> Resources
        </div>
        <div class="card-body">
            <ul class="list-unstyled">
                <li class="mb-2">
                    <a href="#" class="text-decoration-none">
                        <i class="fas fa-file-pdf text-danger me-2"></i>Product Brochure
                    </a>
                </li>
                <li class="mb-2">
                    <a href="#" class="text-decoration-none">
                        <i class="fas fa-file-pdf text-danger me-2"></i>Pricing Guide
                    </a>
                </li>
                <li class="mb-2">
                    <a href="#" class="text-decoration-none">
                        <i class="fas fa-video text-primary me-2"></i>Product Demo
                    </a>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script>
        // Product card hover effects
        document.addEventListener('DOMContentLoaded', function() {
            const productCards = document.querySelectorAll('.product-card');
            productCards.forEach(card => {
                card.addEventListener('mouseenter', function() {
                    this.style.transform = 'scale(1.05)';
                    this.style.transition = 'transform 0.3s ease';
                });
                card.addEventListener('mouseleave', function() {
                    this.style.transform = 'scale(1)';
                });
            });
        });
    </script>
</asp:Content>
