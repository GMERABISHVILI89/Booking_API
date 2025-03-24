using Booking_API;
using Booking_API.EmailHelper;
using Booking_API.FileUploadOperationFilters;
using Booking_API.Interfaces;
using Booking_API.Middlewares;
using Booking_API.Models;
using Booking_API.Services;
using Booking_API.ServicesEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// for prevent cycles
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Prevents circular reference errors
        options.JsonSerializerOptions.WriteIndented = true; // Pretty-print JSON (optional)
    });

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});




builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


// Register AutoMapper with assembly scanning
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


//middleware service added
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Swagger/OpenAPI Configuration with JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Booking", Version = "v2" });

    // Add support for file uploads in Swagger
    c.OperationFilter<SwaggerFileUploadFilter>();

    c.EnableAnnotations();
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddTransient<IEmailService, EmailService>();


// Added DI services 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();




//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
       .AddJwtBearer(options =>
       {
           var jwtSettings = builder.Configuration.GetSection("JwtSettings");
           options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = jwtSettings["ValidIssuer"],
               ValidAudience = jwtSettings["ValidAudience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
           };
       });

builder.Services.AddAuthorization();

// Add this line to make IHttpContextAccessor available
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// Enable CORS
app.UseCors("AllowAngularApp");


// Enables serving wwwroot files
app.UseStaticFiles(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
