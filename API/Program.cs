using API.middleware;
using Application.activities.Queries;
using Application.activities.Validators;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    //add authorization filter instead add it manually at controller
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
/*******************Add Entity framework service configuration Cookies and jwt bearer ******************/
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
/*******************Add Cors for client side app ******************/
builder.Services.AddCors();

/*******************Add MediatR service configuration******************/
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

/*******************Add auto mapper service configuration******************/
builder.Services.AddAutoMapper(options => options.AddMaps(typeof(MappingProfiles).Assembly));

/*******************Add Fluent validations service configuration******************/
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();

/*******************Add dependency injection for exception midlleware******************/
builder.Services.AddTransient<ExceptionMiddleware>();

/*******************Add Idebtity service configuration Cookies and jwt bearer ******************/
builder.Services.AddIdentityApiEndpoints<UserApplication>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite=SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});
#region  handle Jwt and cookie  configuration  once 
//var keyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "EMADISMAILMOHAMMEDDOIN57600181095FARIDADALIDA");
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie(options =>
//{
//    options.Cookie.SameSite = SameSiteMode.None;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.ExpireTimeSpan = TimeSpan.FromDays(7);
//    options.SlidingExpiration = true;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issure"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
//        ClockSkew = TimeSpan.FromMinutes(5), // فترة السماح  اللي بتراعي فرق التوقيت بين السرفرات الاجهزة اللي بتصدر التوكين واللي بتتحقق منه 
//    };
//});
#endregion

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.WithOrigins(builder.Configuration.GetValue<string>("frontend_url") ?? "" ?? "")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup("api").MapIdentityApi<UserApplication>();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
try
{
    var context = service.GetRequiredService<AppDbContext>();
    var userManager = service.GetRequiredService<UserManager<UserApplication>>();
    await DbInitializer.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError("An Error occured during seed data to database (" + ex.InnerException?.Message + ") ");
}
app.Run();
