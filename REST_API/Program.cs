using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using REST_API.Data;
using REST_API.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository with Dependency Injection
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep property names as-is
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Management REST API",
        Version = "v1",
        Description = "A RESTful API for managing students using ASP.NET Core, Entity Framework Core, and Repository Pattern",
        Contact = new OpenApiContact
        {
            Name = "Student Management System",
            Email = "support@studentapi.com"
        }
    });

    // Include XML comments for Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API V1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at root
    });
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Welcome endpoint
app.MapGet("/api", () => Results.Ok(new
{
    message = "Welcome to Student Management REST API",
    version = "1.0",
    documentation = "/swagger",
    endpoints = new
    {
        getAllStudents = "GET /api/students",
        getStudentById = "GET /api/students/{id}",
        createStudent = "POST /api/students",
        updateStudent = "PUT /api/students/{id}",
        deleteStudent = "DELETE /api/students/{id}",
        searchStudents = "GET /api/students/search?searchTerm=...",
        getStudentsByCourse = "GET /api/students/course/{course}",
        getActiveStudents = "GET /api/students/active",
        getTotalCount = "GET /api/students/count"
    }
}))
.WithName("Welcome")
.WithTags("Info");

app.Run();
