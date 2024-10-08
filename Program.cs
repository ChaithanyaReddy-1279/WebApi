using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyWebApi.Data;
using MyWebApi.Mappings;
using MyWebApi.Middlewares;
using MyWebApi.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
 .WriteTo.Console()
 .MinimumLevel.Information()
 .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo{ Title = "NZ Walks API", Version = "v1"});
  options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
  {
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = JwtBearerDefaults.AuthenticationScheme
  });

  options.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = JwtBearerDefaults.AuthenticationScheme
        },
        Scheme = "Oauth2",
        Name = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header
      },
      new List<string>()
    }
  });
});

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));

builder.Services.AddDbContext<NzWalksAuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//Setting up the Identity
builder.Services.AddIdentityCore<IdentityUser>()
       .AddRoles<IdentityRole>()
       .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
       .AddEntityFrameworkStores<NzWalksAuthDbContext>()
       .AddDefaultTokenProviders();

//Setting up the options for the Identity
builder.Services.Configure<IdentityOptions>(options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 6;
  options.Password.RequiredUniqueChars = 1;

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jwt:Key"))
         });
         

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set to empty string to serve Swagger UI at the app's root
});

}


app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
  RequestPath = "/Images"
});
app.UseDefaultFiles();

app.UseAuthentication();
app.UseAuthorization();

// Map other controllers
app.MapControllers();

// Start the application
app.Run();
