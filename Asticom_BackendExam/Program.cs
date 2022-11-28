using Asticom_BackendExam.Context;
using Asticom_BackendExam.Helper;
using Asticom_BackendExam.Models;
using Asticom_BackendExam.Models.DbModel;
using Asticom_BackendExam.UserManagementService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<AdminInfo, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AsticomContext>();

//Add authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(token =>
          {
              token.RequireHttpsMetadata = false;
              token.SaveToken = true;
              token.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                         builder.Configuration.GetSection("Tokens:Key").Value)),
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
          });


builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapperProfile>(), typeof(Program));

string sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AsticomContext>(options =>
    options.UseMySql(sqlConnection, ServerVersion.AutoDetect(sqlConnection)));

builder.Services.AddTransient<Seeder>();
builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.<br>
                      Enter 'Bearer' [space] and then your token in the text input below.<br>
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//Run the seed
var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
if(scopeFactory != null)
{
    using (var scope = scopeFactory.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetService<Seeder>();
        if(seeder != null) seeder.SeedAsync().Wait();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
