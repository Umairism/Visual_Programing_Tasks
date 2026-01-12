# ASP.NET Web Forms Master Page Demo

## 📄 Master Page Architecture

This project demonstrates the use of **Master Pages** in ASP.NET Web Forms to create a consistent layout across multiple web pages. Master Pages provide a template-based approach to building web applications with shared elements like headers, navigation, and footers.

---

## 🏗️ Project Structure

```
MasterPage_Demo/
│
├── Site.Master                    # Master Page (Layout Template)
├── Site.Master.cs                 # Master Page Code-Behind
├── Site.Master.designer.cs        # Designer File
│
├── Default.aspx                   # Home Page (Content Page)
├── About.aspx                     # About Page (Content Page)
├── Contact.aspx                   # Contact Page (Content Page)
├── Products.aspx                  # Products Page (Content Page)
├── Services.aspx                  # Services Page (Content Page)
│
├── Styles/
│   └── site.css                   # Custom Styles
│
├── Web.config                     # Configuration
└── README.md                      # Documentation
```

---

## 🎯 Master Page Concepts

### What is a Master Page?

A **Master Page** is a template that defines the layout and standard elements for multiple pages in an ASP.NET Web Forms application. It provides:

- **Consistent Layout**: Same header, navigation, and footer across all pages
- **Code Reusability**: Write once, use everywhere
- **Easy Maintenance**: Update layout in one place
- **Content Placeholders**: Define areas where content pages can inject their content

### Key Components

#### 1. **Master Page** (`Site.Master`)
The template file that contains:
- Common HTML structure
- Navigation menu
- Header and footer
- `ContentPlaceHolder` controls

```aspx
<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Site.Master.cs" Inherits="MasterPage_Demo.Site" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title><%: Page.Title %></title>
    <!-- Common head content -->
    <asp:ContentPlaceHolder ID="head" runat="server">
    </asp:ContentPlaceHolder>
</head>
<body>
    <!-- Common layout -->
    <asp:ContentPlaceHolder ID="MainContent" runat="server">
    </asp:ContentPlaceHolder>
</body>
</html>
```

#### 2. **Content Pages** (`Default.aspx`, `About.aspx`, etc.)
Pages that inherit from the Master Page:

```aspx
<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasterPage_Demo.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Page-specific head content -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page-specific body content -->
</asp:Content>
```

#### 3. **ContentPlaceHolder Controls**
Define regions where content pages can insert content:

```aspx
<asp:ContentPlaceHolder ID="MainContent" runat="server">
    <!-- Default content (optional) -->
</asp:ContentPlaceHolder>
```

---

## 📊 ContentPlaceHolder Hierarchy

This project uses **multiple ContentPlaceHolders** for flexibility:

| ContentPlaceHolder | Purpose | Required |
|-------------------|---------|----------|
| `head` | Page-specific meta tags, styles, scripts | No |
| `BreadcrumbPlaceHolder` | Breadcrumb navigation | No |
| `PageTitlePlaceHolder` | Page title with icon | Yes |
| `MainContent` | Main page content | Yes |
| `SidebarContent` | Sidebar widgets | No |
| `ScriptsContent` | Page-specific JavaScript | No |

---

## 🎨 Features

### Master Page Features

1. **Responsive Navigation**
   - Bootstrap-based navigation menu
   - Mobile-friendly hamburger menu
   - Active menu highlighting
   - Search functionality

2. **Dynamic Breadcrumbs**
   - Shows current page location
   - Configurable per page

3. **Consistent Header & Footer**
   - Company branding
   - Contact information
   - Social media links
   - Current date/time display

4. **Reusable Code-Behind Methods**
   ```csharp
   public void ShowMessage(string message, string messageType = "info")
   {
       // Display notification to user
   }
   ```

### Content Pages Features

#### **Default.aspx** (Home Page)
- Hero section with call-to-action
- Feature cards with icons
- Statistics display
- Latest news section
- Custom sidebar with updates

#### **About.aspx**
- Company story
- Mission, vision, and values
- Team members (using Repeater control)
- Company timeline
- Achievement badges

#### **Contact.aspx**
- Contact form with validation
- Required field validators
- Email format validation
- Form submission handling
- Contact information cards
- Map placeholder

#### **Products.aspx**
- Product catalog with filtering
- Category-based filtering (All, Cloud, Software, Security)
- Dynamic product cards (using Repeater)
- Product details modal
- Price and popularity badges

#### **Services.aspx**
- Service offerings cards
- Process workflow visualization
- Customer testimonials
- Pricing information
- Call-to-action sections

---

## 🔧 Setup Instructions

### Prerequisites
- Visual Studio 2019/2022
- .NET Framework 4.7.2 or higher
- IIS or IIS Express

### Installation Steps

1. **Open Project in Visual Studio**
   ```
   File > Open > Project/Solution
   Navigate to: MasterPage_Demo.csproj
   ```

2. **Restore NuGet Packages** (if any)
   ```
   Right-click Solution > Restore NuGet Packages
   ```

3. **Build Solution**
   ```
   Build > Build Solution (Ctrl + Shift + B)
   ```

4. **Run Application**
   ```
   Press F5 or click Start button
   ```

5. **View in Browser**
   ```
   Application will open at: http://localhost:[port]/Default.aspx
   ```

---

## 💻 Code Examples

### Accessing Master Page from Content Page

```csharp
// In content page code-behind
protected void Page_Load(object sender, EventArgs e)
{
    if (Master is Site masterPage)
    {
        masterPage.ShowMessage("Welcome!", "success");
    }
}
```

### Dynamically Setting Page Title

```csharp
// In content page
protected void Page_Load(object sender, EventArgs e)
{
    Page.Title = "My Custom Title";
}
```

### Working with ContentPlaceHolder

```aspx
<!-- In content page -->
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Page description" />
    <style>
        /* Page-specific styles */
    </style>
</asp:Content>
```

### Data Binding in Master Page

```csharp
// In Site.Master.cs
protected void Page_Load(object sender, EventArgs e)
{
    lblCurrentTime.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
    HighlightActiveMenuItem();
}
```

---

## 🎯 Master Page Best Practices

### 1. **Keep Master Pages Simple**
- Focus on layout and navigation
- Avoid complex business logic
- Use content pages for specific functionality

### 2. **Use Multiple ContentPlaceHolders**
- Provides flexibility for content pages
- Allows optional content regions
- Better control over page structure

### 3. **Implement Navigation Highlighting**
```csharp
private void HighlightActiveMenuItem()
{
    string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
    // Set active CSS class based on current page
}
```

### 4. **Expose Useful Methods**
```csharp
public void ShowMessage(string message, string type)
{
    // Display notifications from content pages
}
```

### 5. **Handle Page Titles Dynamically**
```aspx
<title><%: Page.Title %> - Company Name</title>
```

### 6. **Use ViewState Judiciously**
- Store minimal data in ViewState
- Consider Session or Cache for larger data

### 7. **Optimize Performance**
- Minimize database calls in Master Page
- Use caching for static content
- Load scripts at bottom when possible

---

## 🔄 Master Page vs Other Approaches

| Feature | Master Pages | MVC Layout | Razor Pages Layout |
|---------|-------------|------------|-------------------|
| Framework | Web Forms | MVC | Razor Pages |
| File Extension | .master | .cshtml | .cshtml |
| Content Areas | ContentPlaceHolder | @RenderBody() | @RenderBody() |
| Code-Behind | Yes | No | Yes (Page Model) |
| Designer Support | Yes | Limited | Limited |
| ViewState | Yes | No | No |

---

## 📱 Responsive Design

The application is fully responsive using Bootstrap 5:

- **Mobile First**: Optimized for mobile devices
- **Breakpoints**: xs, sm, md, lg, xl
- **Navigation**: Collapsible menu on mobile
- **Grid System**: Flexible column layouts
- **Utilities**: Spacing, alignment, display helpers

---

## 🎨 Customization

### Changing Theme Colors

Edit `Styles/site.css`:

```css
.btn-primary {
    background: linear-gradient(135deg, #your-color1, #your-color2);
}
```

### Adding New ContentPlaceHolder

1. In `Site.Master`:
```aspx
<asp:ContentPlaceHolder ID="NewPlaceHolder" runat="server">
    <!-- Default content -->
</asp:ContentPlaceHolder>
```

2. In content pages:
```aspx
<asp:Content ID="Content" ContentPlaceHolderID="NewPlaceHolder" runat="server">
    <!-- Custom content -->
</asp:Content>
```

### Modifying Navigation

Edit the navigation section in `Site.Master`:

```aspx
<ul class="navbar-nav">
    <li class="nav-item">
        <asp:HyperLink ID="lnkNewPage" runat="server" NavigateUrl="~/NewPage.aspx" CssClass="nav-link">
            <i class="fas fa-icon me-1"></i>New Page
        </asp:HyperLink>
    </li>
</ul>
```

---

## 🐛 Troubleshooting

### Master Page Not Found
**Error**: Could not load master page  
**Solution**: Check `MasterPageFile` path in `@Page` directive

### ContentPlaceHolder Not Recognized
**Error**: Content control has to be top-level  
**Solution**: Ensure `<asp:Content>` is direct child of page

### ViewState Issues
**Error**: ViewState validation failed  
**Solution**: Set `EnableViewStateMac="false"` (not recommended for production)

### Styling Not Applied
**Issue**: CSS not loading  
**Solution**: Check path in `<link>` tag, use `~` for root-relative paths

---

## 📚 Learning Objectives

After exploring this project, you will understand:

1. ✅ Master Page architecture and purpose
2. ✅ ContentPlaceHolder controls
3. ✅ Content page structure
4. ✅ Master page code-behind functionality
5. ✅ Communication between master and content pages
6. ✅ Dynamic navigation highlighting
7. ✅ Multiple ContentPlaceHolder usage
8. ✅ Responsive design with Bootstrap
9. ✅ Form validation in Web Forms
10. ✅ Data binding with Repeater control

---

## 🚀 Extending the Project

### Add New Pages

1. **Create new .aspx file**
2. **Set MasterPageFile**:
   ```aspx
   <%@ Page Title="New Page" MasterPageFile="~/Site.Master" %>
   ```
3. **Add Content controls**
4. **Update navigation** in Site.Master

### Implement Nested Master Pages

```aspx
<!-- SubMaster.master -->
<%@ Master Language="C#" MasterPageFile="~/Site.Master" %>
```

### Add Authentication

```xml
<!-- Web.config -->
<authentication mode="Forms">
    <forms loginUrl="~/Login.aspx" />
</authentication>
```

---

## 📖 Additional Resources

- [ASP.NET Master Pages Overview](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/older-versions-getting-started/master-pages/)
- [ContentPlaceHolder Control](https://docs.microsoft.com/en-us/dotnet/api/system.web.ui.webcontrols.contentplaceholder)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.3/)
- [Font Awesome Icons](https://fontawesome.com/icons)

---

## 📝 Key Takeaways

1. **Master Pages** provide consistent layout across multiple pages
2. **ContentPlaceHolder** defines customizable content regions
3. **Content Pages** inherit master page layout and override placeholders
4. **Code-Behind** in master pages handles common functionality
5. **Multiple ContentPlaceHolders** offer flexibility
6. **Responsive Design** ensures mobile compatibility
7. **Bootstrap Integration** simplifies UI development

---

## 🎓 Project Completion

This Master Page demo project demonstrates:
- ✅ Complete master page implementation
- ✅ Multiple content pages with different layouts
- ✅ Dynamic navigation with active state
- ✅ Breadcrumb navigation
- ✅ Form validation
- ✅ Data binding with Repeater control
- ✅ Responsive design
- ✅ Master-Content page communication
- ✅ Professional styling with Bootstrap 5

**Happy Coding! 🎉**
