using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using pmt_auth.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Add S3 service client to dependency injection container
builder.Services.AddAWSService<IAmazonS3>();

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
string dbConn = $"Host={builder.Configuration.GetValue<string>("DBHost")};Port={builder.Configuration.GetValue<int>("DBPort")};Database={builder.Configuration.GetValue<string>("DBName")};Username={builder.Configuration.GetValue<string>("DBUser")};Password={builder.Configuration.GetValue<string>("DBPassword")};";
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

// CORS setup
app.UseCors(options => options
  .AllowAnyMethod()
  .AllowAnyHeader()
  .SetIsOriginAllowed(origin => true)
  .AllowCredentials()
);


app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
