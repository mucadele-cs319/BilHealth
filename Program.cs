using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Services;
using BilHealth.Services.Users;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.JsonNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddNewtonsoftJson(o => o.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"), o => o.UseNodaTime()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IPasswordHasher<AppUser>, BCryptPasswordHasher>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});

builder.Services.AddDataProtection().PersistKeysToStackExchangeRedis(
    () => StackExchange.Redis.ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")).GetDatabase(),
    "DataProtection-Keys");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "bilhealthsess";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
}); // May want to use a server-side ticket instead: https://mikerussellnz.github.io/.NET-Core-Auth-Ticket-Redis/

builder.Services.AddSingleton<IClock>(SystemClock.Instance);

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "BilHealth API", Version = "v1" }); });
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<IAuthenticationService>().CreateRoles().Wait();
}

// TODO: A proper system initialization flow with default user creation
using (var scope = app.Services.CreateScope())
{
    // Register an admin user
    var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
    var adminUsername = "0000";

    authService.DeleteUser(adminUsername).Wait();
    authService.Register(new()
    {
        UserName = adminUsername,
        Password = "admin123",
        Email = "tempmail@example.com",
        FirstName = "John",
        LastName = "Smith",
        UserType = UserRoleType.Admin
    }).Wait();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. See https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // For production scenarios, it may make more sense to use a reverse-proxy instead of Kestrel-based HTTPS
    // See the comments in file `docker-compose.prod.yml` for details
    // Although, is this needed even in development where the edge server is CRA's live Express server?
    app.UseHttpsRedirection();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "BilHealth API v1");
    });
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
