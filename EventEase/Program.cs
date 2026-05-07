using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC Services (Controllers and Views)
builder.Services.AddControllersWithViews();

// 2. Add SQL Database Service (Entity Framework)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Add Image Storage Service (Blob Service for Azurite)
// Note: If you haven't created the Services folder/files yet, 
// you can comment out the line below with // to make the app run.
builder.Services.AddScoped<IBlobService, BlobService>();

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

// 4. Set the Homepage route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();