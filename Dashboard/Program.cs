using System.Runtime.InteropServices;
using BRSSinnar.Dashboard.Helpers;
using Core.Entities;
using Dashboard.MappingProfiles;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<TSTDBContext>();
builder.Services.AddAutoMapper(typeof(ViewModelsMappingProfiles));
builder.Services.AddTransient<SMSService, SMSService>();
builder.Services.AddTransient<ImageUploadService, ImageUploadService>();
builder.Services.AddTransient<ImageProcessingService, ImageProcessingService>();
builder.Services.AddTransient<FacultyClaimsService, FacultyClaimsService>();

builder.Services.AddTransient(provider =>
{
    // Resolve ILogger<FirebaseService> from the DI container
    var logger = provider.GetRequiredService<ILogger<FirebaseService>>();


    // Get the CredentialFilePath from configuration or hardcode it
    string credentialFilePath = "";

    if (builder.Environment.IsDevelopment())
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            credentialFilePath = builder.Configuration["Firebase:MacOS:Credentials"];
        }
        else
        {
            credentialFilePath = builder.Configuration["Firebase:Windows:Credentials"];
        }
    }
    else
    {
        credentialFilePath = builder.Configuration["Firebase:Credentials"];

    }



    // Create and return an instance of FirebaseService
    return new FirebaseService(logger, credentialFilePath);
});
builder.Services.AddIdentity<APIUser, Role>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<TSTDBContext>()
                .AddDefaultTokenProviders();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
    //Notifications Policy
    options.AddPolicy("NotificationsClaims", policy =>
    policy.RequireClaim("Permission", "ViewNotifications")
          .RequireClaim("Permission", "CreateNotification")
          .RequireClaim("Permission", "EditNotifications"));
    //Notifications Policy
    options.AddPolicy("ServicesClaims", policy =>
    policy.RequireClaim("Permission", "ViewServices")
          .RequireClaim("Permission", "CreateServies")
          .RequireClaim("Permission", "EditServices"));

    options.AddPolicy("AnnouncementsClaims", policy =>
policy.RequireClaim("Permission", "ViewAnnouncements")
      .RequireClaim("Permission", "CreateAnnouncement")
      .RequireClaim("Permission", "EditAnnouncements"));


    options.AddPolicy("StudentProfilesClaims", policy =>
policy.RequireClaim("Permission", "ViewStudentsProfiles"));

    options.AddPolicy("StudentsMobilesClaims", policy =>
policy.RequireClaim("Permission", "ViewStudentMobiles"));


    options.AddPolicy("SubjectManagementClaims", policy =>
policy.RequireClaim("Permission", "ViewSubjectManagement"));

    options.AddPolicy("UserManagementClaims", policy =>
policy.RequireClaim("Permission", "ViewUsers"));


    options.AddPolicy("TicketManagementClaims", policy =>
policy.RequireClaim("Permission", "ViewSupportTickets"));

});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/";
    options.AccessDeniedPath = "/Error/403";
});


//builder.Services.AddSignalR();

var app = builder.Build();

await using (var context = new TSTDBContext())
{

    using (var scope = app.Services.CreateScope())
    {
        UserManager<APIUser> _userManager = scope.ServiceProvider.GetService<UserManager<APIUser>>();
        if (_userManager != null)
        {
            var user = await _userManager.FindByNameAsync("administrator");
            if (user == null)
            {
                var _password = "w$4LK29rYQ0c";
                user = new APIUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "administrator",
                    NormalizedUserName = "ADMINISTRATOR",
                    Email = "admin@merowe.edu.sd",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user, _password);
                if (result.Succeeded)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");

                }

            }
        }



    }


    var SystemClaims = context.SystemClaims.ToList();

    if (SystemClaims.Any())
    {

    }
    else
    {


        var systemClaimsInitial = new List<SystemClaim>
{
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewAnnouncements", Description = "Allow the user to view the announcments module" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewServices", Description = "View Service" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewDashboard", Description = "View Dashboard analysis" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewNotifications", Description = "View Notifications Module" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "CreateNotification", Description = "Create a new Notification" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "EditNotifications", Description = "Edit Notifications" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "CreateAnnouncement", Description = "Allow the users to create announcements" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "EditAnnouncements", Description = "Allow the users to edit announcements" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "CreateServies", Description = "Allow the users to view the services" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "EditServices", Description = "Allow the users to edit services" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewStudentsProfiles", Description = "View Students Profiles" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewStudentMobiles", Description = "All the user to View Students Mobiles page" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewSubjectManagement", Description = "Allow the user to access Subject management" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewUsers", Description = "Allow the user to view the list of system users" },
    new SystemClaim { ClaimType = "Permission", ClaimValue = "ViewSupportTickets", Description = "Allow the users to view support tickets" },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "11-Computer Science and Information Technology", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "12-Medicine and Surgery", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "13-Dental Medicine and Surgery", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "14-Clinical and Industrial Pharmacy", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "15-Nursing and Midwifery", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "16-Administrative Sciences", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "17-physiotherapy", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "18-Radiography and Medical Imaging Sciences", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "19-Medical Laboratory Science", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "20-International Relations & Diplomatic Studies", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "21-Engineering and Architecture", Description = null },
    // new SystemClaim { ClaimType = "Faculty", ClaimValue = "22-Graduation College", Description = null },
};



        context.SystemClaims.AddRange(systemClaimsInitial);

        await context.SaveChangesAsync();
    }






}



// For status codes like 401, 403, 404
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseSession();

app.UseStaticFiles();


if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();

    var ImagesPath = "";

    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        ImagesPath = builder.Configuration["ImagesFolder:MacOS"];

    }
    else
    {
        ImagesPath = builder.Configuration["ImagesFolder:Windows"];

    }

    //var externalFolderPath = "D:\\Images"; // Change this path
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(ImagesPath),
        RequestPath = "/images"
    });

}


app.UseRouting();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    // Map both Hub and controllers
   // endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}");
});

app.Run();


