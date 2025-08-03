
using API.HttpServices;
using BRSSinnar.Dashboard.Helpers;
using Core.Entities;
using HttpServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var configuration  = new ConfigurationBuilder().AddJsonFile("appsettings.json")
     .Build();



Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
// Add Security Definition for JWT Bearer Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your_token}'"
    })

    );
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IJWTService, JWTService>();
builder.Services.AddTransient<ImageValidationService, ImageValidationService>();
builder.Services.AddTransient<SMSService, SMSService>();
builder.Services.AddTransient<ImageUploadService, ImageUploadService>();
builder.Services.AddTransient<ImageProcessingService, ImageProcessingService>();
builder.Services.AddTransient<FileUploadService, FileUploadService>();
builder.Services.AddTransient<StudentDetailsService, StudentDetailsService>();
builder.Services.AddDbContext<TSTDBContext>();
builder.Services.AddScoped<FacultyClaimsService, FacultyClaimsService>();
builder.Services.AddHttpClient();



builder.Services.AddIdentity<APIUser, Role>(
                options =>
                {
                    options.User.AllowedUserNameCharacters = "0123456789";
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<TSTDBContext>()
                .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
                )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader();
                      });
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:5013") // Your MVC app's URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Important for SignalR
    });
});



await using (var context = new TSTDBContext())
{

    var passwordHasher = new PasswordHasher<APIUser>();




    var services = await context.Services.ToListAsync();
    if (services.Any())
    {
    }
    else
    {
        var servicesList = new List<Service> {
            new Service{
                Id = Guid.NewGuid().ToString(),
                Name = "New ID Card",
                NameAr ="بطاقة جامعية",
                Fee = 5000.0,
                CreatedAt = DateTime.Now
            },
            new Service{
             Id = Guid.NewGuid().ToString(),
                Name = "Transcript Certificate",
                NameAr ="شهادة تفاصيل",
                Fee = 6000.0,
                CreatedAt = DateTime.Now
            },
            new Service{
             Id = Guid.NewGuid().ToString(),
                Name = "General Certificate",
                NameAr ="شهادة عامة",
                Fee = 6000.0,
                CreatedAt = DateTime.Now
            },
        new Service{
             Id = Guid.NewGuid().ToString(),
                Name = "Enrollment Certificate",
                NameAr ="شهادة قيد",
                Fee = 2500.0,
                CreatedAt = DateTime.Now
            },
        };
        await context.Services.AddRangeAsync(servicesList);
    }


    var requestStatuses = await context.RequestStatuses.ToListAsync();
    if (requestStatuses.Any())
    {

    }
    else {

        var requestStatusesList = new List<RequestStatus> {
        new RequestStatus
            {
                Id = 5,
        Name = "Pending Payment"
        }
            ,
        new RequestStatus  {
            Id = 6,
        Name = "Confirmed"
        }
            ,
        new RequestStatus {
            Id = 7,
        Name = "Ready"
        }
            ,
        new RequestStatus {
            Id = 8,
        Name = "Collected"
        }
            ,
            new RequestStatus {
                Id  = 9,
        Name = "Rejected"
        }
            ,
        new RequestStatus {
            Id = 10,
        Name = "Pending Verification"
        }
            ,
            new RequestStatus {
                Id = 11,
        Name = "Approved"
        }
            ,
            new RequestStatus {
        Id = 12,
        Name = "Printed"
        }
            ,
            new RequestStatus {
        Id = 13,
        Name = "Recieved"
        }
            ,
    };

        await context.RequestStatuses.AddRangeAsync(requestStatusesList);

    }
    var dbrole = context.Roles.FirstOrDefault(e => e.Name == "Student");
    var adminDBrole = context.Roles.FirstOrDefault(e => e.Name == "Admin");

    if (dbrole == null) {
        var Studentrole = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Student",
            NormalizedName = "STUDENT"
        };
       
        context.Roles.Add(Studentrole);
    }
    if (adminDBrole == null) {
        var adminRole = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Admin",
            NormalizedName= "admin".ToUpper()
        };
        context.Roles.Add(adminRole);
    }

    var religions = await context.Religions.ToListAsync();
    if (!religions.Any()) {
        List<Religion> rel = new List<Religion>
        {

            new Religion{
            Id= Guid.NewGuid().ToString(),
            Name = "مسلم"
            },
            new Religion
            {
                Id= Guid.NewGuid().ToString(),
                Name = "مسيحي"
            },
             new Religion
            {
                Id= Guid.NewGuid().ToString(),
                Name = "أخرى"
            }


        };
        await context.Religions.AddRangeAsync(rel);
    }

    await context.SaveChangesAsync();
}
builder.Host.UseSerilog();
builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();


