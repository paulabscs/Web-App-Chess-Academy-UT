using Microsoft.EntityFrameworkCore;
using UT.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UTDatabase") ?? throw new InvalidOperationException("Connection string 'UTDatabase' not found.");

builder.Services.AddScoped<IUTRepo, UTRepo>();
builder.Services.AddDbContext<UTDBContext>(options => options.UseSqlite(connectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(17); 
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Setting cache control headers to prevent caching
        ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate"; 
        ctx.Context.Response.Headers["Pragma"] = "no-cache"; 
        ctx.Context.Response.Headers["Expires"] = "0";
    }
});

app.UseSession();
app.UseRouting();
app.UseAuthorization(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("/html/index.html");
});

app.Run();
