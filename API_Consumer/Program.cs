using API_Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient with base address
builder.Services.AddHttpClient<IStudentApiService, StudentApiService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001/api";
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    
    // Set timeout
    var timeout = configuration.GetValue<int>("ApiSettings:Timeout", 30);
    client.Timeout = TimeSpan.FromSeconds(timeout);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
