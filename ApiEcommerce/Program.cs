using System.Text;
using ApiEcommerce.Constants;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Mapster;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(dbConnectionString)
  .UseSeeding((context, _) =>
  {
    var appContext = (ApplicationDbContext)context;
      DataSeeder.SeedData(appContext);    
  })
);

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024;// esta multiplicacion da el valor y tamaño de 1Mega ya que se esta indicando en bytes
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Configuración de Mapster
ApiEcommerce.Mapping.MapsterConfig.RegisterMappings();
builder.Services.AddMapster();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Secret key no esta generada");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // quitar la necesidad de usar htps
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // para hacer la firma del token con la llave secreta
        ValidateIssuer = false, // no se va validar el emisor del token 
        ValidateAudience = false //para que no valide el publico
    };
});

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
    options.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);    
}
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    // option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version")); // se va agregar el parametro en la url como: ?api-version
});
apiVersioningBuilder.AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV"; // v1,2,v3...
    option.SubstituteApiVersionInUrl = true; //api/v{ersion}/products
});

// CORS: para permitir o restringir peticiones de diferentes origenes externos
builder.Services.AddCors(options =>
{
    options.AddPolicy(PolicyNames.AllowSpecificOrigin,
    builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //  app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI();

    // Configuración de Scalar con opciones personalizadas
    app.MapScalarApiReference(options =>
    {
        options.Title = "API Documentation";
        options.Theme = ScalarTheme.BluePlanet;
        options.DarkMode = true;
    });

    

    // Endpoints específicos para cada versión
    app.MapScalarApiReference("/v1", options =>
    {
        options
            .WithTitle("Mi API v1.0 - Documentación")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithOpenApiRoutePattern("/api-docs/v1/swagger.json")
            .WithModels(true);
    });

    app.MapScalarApiReference("/v2", options =>
    {
        options
            .WithTitle("Mi API v2.0 - Documentación")
            .WithTheme(ScalarTheme.Purple)
            .WithOpenApiRoutePattern("/api-docs/v2/swagger.json")
            .WithModels(true);
    });   
}

/*
        los app. son middlewares;
        Un middleware es como un "filtro" en una cadena de procesamiento. 
        Cada petición HTTP pasa a través de múltiples middlewares en orden secuencial.
*/


app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// app.MapScalarApiReference(options =>
// {
//     options.WithTitle("Mi API con Scalar");
//     options.WithLayout(Scalar.AspNetCore.ScalarLayout.Modern);
//     options.WithFavicon("https://example.com/favicon.ico");
// });

app.Run();
