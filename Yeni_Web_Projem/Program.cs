using Microsoft.Extensions.Options;
using Yeni_Web_Projem.Models;
using Yeni_Web_Projem.Services;
using Yeni_Web_Projem.Views;
using static System.Net.WebRequestMethods;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// LDAP Settings Configuration
builder.Services.Configure<LDAPSettings>(
    builder.Configuration.GetSection("LDAPSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register LDAP Settings as Singleton for direct injection
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<LDAPSettings>>().Value);

// HTTP Client Registration:
builder.Services.AddHttpClient<LDAPAuthenticationService>();

// Service Registration:
builder.Services.AddScoped<LDAPAuthenticationService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();