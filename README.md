# My Visual Programming Practice Projects

## ⚠️ IMPORTANT: .NET 10 Compatibility

**Only 4 out of 11 projects** can run on .NET 10. The other 7 are **ASP.NET Web Forms** projects that require **.NET Framework 4.8**.

See [WEBFORMS_NET10_INCOMPATIBILITY.md](WEBFORMS_NET10_INCOMPATIBILITY.md) for full details.

## About This Collection

Hey! This is my collection of **11 .NET projects** that I've been working on to learn different architectures and patterns.

## 📦 All Projects

| # | Project | Type | Technology | .NET 10 Status |
|---|---------|------|------------|----------------|
| 1 | **ADO_WinForms** | Desktop | Windows Forms | ✅ Works |
| 2 | **ADO_CRUD** | Web | Web Forms | ❌ Framework Only |
| 3 | **DbCon_CRUD** | Web | Web Forms | ❌ Framework Only |
| 4 | **ThreeTier_CRUD** | Web | Web Forms | ❌ Framework Only |
| 5 | **StoredProcedure_CRUD** | Web | Web Forms | ❌ Framework Only |
| 6 | **MasterPage_Demo** | Web | Web Forms | ❌ Framework Only |
| 7 | **Auth_WebForms** | Web | Web Forms | ❌ Framework Only |
| 8 | **Auth_WebForms_Connected** | Web | Web Forms | ❌ Framework Only |
| 9 | **REST_API** | API | ASP.NET Core | ✅ Works |
| 10 | **API_Consumer** | Console | .NET Core | ✅ Works |
| 11 | **Core_MVC_EF** | Web | ASP.NET Core MVC | ✅ Works |

**Legend:**
- ✅ **Works on .NET 10** - Modern .NET Core/.NET 5+ compatible
- ❌ **Framework Only** - Requires .NET Framework 4.7.2/4.8 (Windows only)

## How to Open

There are a few ways to open these projects:

**Easiest way:** Run the PowerShell script I made
```powershell
.\Open-Projects.ps1
```

**Quick way:** Just double-click `Taska.sln` (note: Web Forms projects will show build errors)

**Command line:**
```powershell
cd "e:\Visual Programing Practice\Taska"
start Taska.sln
```

**To build only compatible projects:**
```powershell
dotnet build ADO_WinForms/ADO_WinForms.csproj
dotnet build REST_API/REST_API.csproj
dotnet build API_Consumer/API_Consumer.csproj
dotnet build Core_MVC_EF/Core_MVC_EF.csproj
```

## ✅ What's Included
What's in Each Project

### 1. **ADO_WinForms** - Windows Forms Desktop (.NET 10) ✅
This is a desktop app using pure ADO.NET (no Entity Framework or anything fancy). It has:
- MainForm showing statistics dashboard
- StudentListForm with DataGridView for viewing students
- Add/Edit forms that pop up as modal dialogs
- Course management features

### 2. **ADO_CRUD** - Pure ADO.NET Web (.NET Framework 4.8) ❌
```
Direct database access with ADO.NET
Same ADO.NET approach but for web applications. Learning how to use:
- SqlConnection, SqlCommand, SqlDataReader directly
- SqlDataAdapter for GridView binding
- Student and Course CRUD operations
- Basic search and filtering 3. **DbCon_CRUD** - Centralized Utility
```
Reusable DbCon static class
Here I created a reusable DbCon class to avoid repeating database code everywhere:
- ExecuteNonQuery, ExecuteScalar methods
- ExecuteReader, ExecuteDataTable helpers
- Product and Category management
- Even has transaction support 4. **ThreeTier_CRUD** - 3-Tier Architecture
```
Proper separation of concerns
This one follows proper architecture with separate layers:
- Data Access Layer (DAL) - handles database stuff
- Business Logic Layer (BLL) - business rules and validation
- Presentation Layer (UI) - the web pages
- Built an employee management system with this 5. **StoredProcedure_CRUD** - Database Procedures
```
SQL stored procedures for all operUsing Stored Procedures
Learning how to work with stored procedures instead of inline SQL:
- Created procedures like sp_GetAllBooks, sp_InsertBook, etc.
- Using SqlCommand with CommandType.StoredProcedure
- Made a simple library management system
- Good for understanding database-side logic 6. **MasterPage_Demo** - Consistent Layouts
```
Reusable master pagesMaster Pages
Figuring out how master pages work in Web Forms:
- Site.Master for consistent navigation
- Content pages that inherit from the master
- Header/footer stays same across all pages
- Using ContentPlaceHolder properly 7. **Auth_WebForms** - Authentication
```
Custom authentication system
✓ User registration with validation
✓ Login/Logout functionality System
Built my own authentication from scratch:
- User registration with validation
- Login/Logout pages
- Using BCrypt for password hashing (important for security!)
- Session management to keep users logged inabase-backed authentication
✓ Users stored in SQL Server
✓ Role-based accessentication
Similar to #7 but storing users in a database:
- All user info in SQL Server
- Added role-based access control
- "Remember me" checkbox that actually works
- User profile pagesern REST API with EF Core
✓ CRUD endpoints for Products
✓ Swagger documentatioBuilding APIs
My first real REST API using ASP.NET Core:
- CRUD endpoints for Products
- Swagger UI for testing (super helpful!)
- Using Entity Framework Core instead of raw SQL
- Learning async/await properlysuming REST APIs
✓ HttpClient usage
✓ JSON serialization/deseriConsuming APIs
Learning how to call APIs from a web app:
- Using HttpClient to make requests
- JSON serialization/deserialization (Newtonsoft.Json)
- Showing API data in the UI
- Handling errors when API is downern MVC application
✓ Razor views
✓ Model-View-Controller pattern
✓ Entity Framework Core
✓ Dependency injection
Finally getting into modern ASP.NET Core:
- Razor views for the UI
- Proper MVC pattern (Model-View-Controller)
- Entity Framework Core for database
- Dependency injection (took a while to understand this!)*.NET** | 10.0 | Runtime |
| **C#** | 13.0 | Language |
| *Tech Stack

What I'm using across these projects:

- **.NET 10** - Latest version (just upgraded!)
- **C# 13** - The language features are pretty cool
- **Entity Framework Core 10** - For some projects
- **SQL Server** - My go-to database
- **Visual Studio 2026** - IDE
- **Windows Forms** - For desktop apps
- *Documentation

I've written some guides to help navigate everything:

- *Suggested Learning Order

If you're going through these projects, here's the order I'd recommend:

**Start with these (Basics):**
1. ADO_CRUD - Get comfortable with ADO.NET
2. DbCon_CRUD - Learn to reuse code
3. MasterPage_Demo - Understand layouts

**Then move to (Intermediate):**
4. Auth_WebForms - Security is important
5. StoredProcedure - Database-side logic
6. ADO_WinForms - Desktop development

**Finally (Advanced):**
7. ThreeTier_CRUD - Proper architecture
8. REST_API - Building APIs
9. API_Consumer - Calling APIs
10. Core_MVC_EF - Modern development

## 🔍 Key Concepts Demonstrated

### Architecture Patterns
- ✅ Pure ADO.NET (simple, direct)
- ✅ Centralized utility class (reusable)
- ✅ 3-Tier architecture (scalable)
- ✅ MVC pattern (modern)
- ✅ REST API (service-oriented)

### Database Access
- ✅ SqlConnection, SqlCommand
- ✅ SqlDataReader (forward-only)
- ✅ SqlDataAdapter (DataSet/DataTable)
- ✅ Stored Procedures
- ✅ Entity Framework Core (ORM)

### UI Technologies
- ✅ Windows Forms (desktop)
- ✅ ASP.NET Web Forms (traditional web)
- ✅ ASP.NET Core MVC (modern web)
- ✅ Master Pages (consistent layouts)
- ✅ Razor views (view engine)

### Security
- ✅ Password hashing (BCrypt)
- ✅ SQL injection prevention (parameters)
- ✅ Session management
- ✅ Authentication/Authorization

## 💻 System Requirements

- *What You Need

To run these projects:

- Windows 10 or 11
- Visual Studio 2026 (or newer)
- .NET 10 SDK
- SFirst Time Setup

Here's what I do when setting up on a new machine:

1. **Open the solution** - Just double-click `Taska.sln`

2. **Restore NuGet packages** - VS will ask you to restore packages, click "Restore"

3. **Setup the databases** - Each project folder has a `Database/SetupDatabase.sql` file. Open these in SQL Server Management Studio and run them.

4. **Build everything** - Press Ctrl + Shift + B to build the whole solution

5. **Pick a project and run it** - Right-click any project, choose "Set as Startup Project", then press F5ht-click project > Set as Startup Project
   Press F5 to run
   ```

## 📊 Project Statistics

```
Total Projects:     11
Total Files:        500+
Lines of Code:      10,000+
Database Tables:    30+
API Endpoints:      20+
Forms/Pages:        50+
```

## 🔥 Features Highlights

### Modern Development
- ✅ SDK-style projects (clean, compact)
- ✅Some Stats

Just for fun, here's what's in this collection:

- 11 complete projects
- Over 500 files
- Around 10,000+ lines of code (probably more by now)
- 30+ database tables
- 20+ API endpoints
- 50+ forms and pages Input validation
- ✅ Code organization

### Educational Value
- ✅What I Learned

**Modern .NET Development:**
- SDK-style projects (so much cleaner!)
- .NET 10 features
- C# 13 language improvements
- Modern NuGet with PackageReference
- Hot reload is actually useful

**Good Practices:**
- Always use parameterized queries (SQL injection is real!)
- Close your database connections properly
- Handle errors gracefully
- Validate user input
- Organize code in a logical way

**Real-World Skills:**
- Different architecture patterns and when to use them
- Starting simple and adding complexity as needed
- Real scenarios you'd face in actual projects
- Writing decent documentation (like this!)
```
Tools > NuGet Package Manager > 
  MCommon Tasks

**Building everything:**
- In Visual Studio: Build > Build Solution
- Command line: `dotnet build`

**Running a specific project:**
- Right-click the project in Solution Explorer
- Click "Set as Startup Project"
- Press F5

**Updating packages:**
- Tools > NuGet Package Manager
- Manage NuGet Packages for Solution
- Check the Updates tab

**Cleaning up:**
- Build > Clean Solution
- Or just delete the bin/ and obj/ folders
2. **Architecture**
   - Layered architecture
   - Separation of concerns
   - MVC pattern
   - API design

3. **Security**
   Useful Links

Some documentation I found helpful:

- Official .NET docs: https://docs.microsoft.com/dotnet
- ASP.NET Core docs: https://docs.microsoft.com/aspnet/core
- Entity Framework: https://docs.microsoft.com/ef/core
- C# Guide: https://docs.microsoft.com/dotnet/csharp

## What These Projects Cover

Working through all of these, you'll get hands-on with:

**Database Stuff:**
- Pure ADO.NET vs using an ORM like Entity Framework
- Managing connections properly
- Stored procedures
- Transactions

**Architecture:**
- Different ways to structure applications
- Separating concerns (not mixing UI and database code)
- MVC pattern
- API design

**Security:**
- Hashing passwords (never store plain text!)
- Building authentication systems
- Preventing SQL injection
- Session management

**Modern .NET:**
- The new SDK-style projects
- Dependency injection
- Async/await programming
- Building REST APIs

## Troubleshooting

If something's not working:
1. Check MIGRATION_TO_NET10.md for common issues
2. Make sure .NET 10 SDK is installed: `dotnet --version`
3. Restore NuGet packages (right-click solution > Restore NuGet Packages)
4. Check your database connection strings in Web.config or App.config

---Quick Checklist

Before you start:
- [ ] Got Visual Studio 2026 installed?
- [ ] .NET 10 SDK installed?
- [ ] SQL Server running (LocalDB is fine)?
- [ ] Solution opens without errors?
- [ ] NuGet packages restored?
- [ ] Projects build successfully?
- [ ] Ran the database setup scripts?

## Getting Started

Pick your method:
```powershell
# Use my helper script
.\Open-Projects.ps1

# Or just open the solution
start Taska.sln

# Or open a specific project
start ADO_WinForms\ADO_WinForms.csproj
```

---

## Status

All 11 projects are now on .NET 10 and working with Visual Studio 2026. The old project files are backed up as *.csproj.old just in case.

Last updated: January 13, 2026

---

That's it! Just open Taska.sln and you're good to go. Feel free to explore any project that interests you.