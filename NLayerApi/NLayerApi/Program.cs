using BusinessLayer.Interfaces;
using BusinessLayer.Services;

//using BusinessLogic.Interfaces;
//using BusinessLogic.Services;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Interceptor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the Stack below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<GeoLocationService>();

//builder.Services.AddDbContext<DataContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("NLayerApi"));
//    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//    //.AddInterceptors(serviceProvider.GetRequiredService<UpdateAuditInterceptor>());
//});

builder.Services.AddDbContext<DataContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DataAccess"))
    .AddInterceptors(serviceProvider.GetRequiredService<UpdateAuditInterceptor>()); //Add intercepters
});

builder.Services.AddScoped<IOrganisationService, BusinessLayer.Services.OrganisationService>();
builder.Services.AddScoped<IAddressService, BusinessLayer.Services.AddressService>();
builder.Services.AddScoped<ICompanyContactService, BusinessLayer.Services.CompanyContactService>();
builder.Services.AddScoped<IDirectorateService, BusinessLayer.Services.DirectorateService>();
builder.Services.AddScoped<IDepartmentService, BusinessLayer.Services.DepartmentService>();
builder.Services.AddScoped<ITeamService, BusinessLayer.Services.TeamService>();
builder.Services.AddScoped<IRegionService, BusinessLayer.Services.RegionService>();
builder.Services.AddScoped<ISupportingMaterialService, BusinessLayer.Services.SupportingMaterialService>();
builder.Services.AddScoped<IContactService, BusinessLayer.Services.ContactService>();

builder.Services.AddScoped<IOrganisationServiceService, BusinessLayer.Services.OrganisationServiceService>();
builder.Services.AddScoped<IOrganisationProgrammeService, BusinessLayer.Services.OrganisationProgrammeService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IProgrammeService, ProgrammeService>();
builder.Services.AddScoped<ICompanyContactService, CompanyContactService>();
builder.Services.AddScoped<CompanyContactService>();
builder.Services.AddScoped<ITrustDistrictService, TrustDistrictService>();
builder.Services.AddScoped<ITrustRegionService, TrustRegionService>();
builder.Services.AddScoped<IGovOfficeRegionService, GovOfficeRegionService>();
builder.Services.AddScoped<IPremisesService, BusinessLayer.Services.PremisesService>();
builder.Services.AddScoped<GeoLocationService>();

builder.Services.AddSingleton<UpdateAuditInterceptor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<GeoLocationService>(); 


builder.Services.AddScoped<IRegionService, BusinessLayer.Services.RegionService>();

builder.Services.AddIdentityCore<User>(opt => {
    opt.User.RequireUniqueEmail = true;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    }
);

builder.Services.AddScoped<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    context.Database.Migrate();
    await DbInitializer.Initialize(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "A problem occurred during migration");
}


app.Run();

