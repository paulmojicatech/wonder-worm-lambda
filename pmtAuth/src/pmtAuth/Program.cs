using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using pmt_auth.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
  options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
  {
    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    Name = "Authorization",
    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
  });
  options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// add auth
builder.Services.AddAuthentication().AddJwtBearer(options => {
  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
  {
    ValidateIssuer = false,
    ValidateAudience = false,    
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey")))
  };
});

// add Postgres
string dbConn = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<PmtAuthContext>(options =>
{
  options.UseNpgsql(dbConn);
});

// add JSON Serializer
builder.Services.Configure<JsonSerializerOptions>(options =>
{
  options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
