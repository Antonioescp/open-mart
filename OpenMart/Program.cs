using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using OpenMart.Data.Context;
using OpenMart.EmailService;
using OpenMart.EmailService.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OpenMartDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OpenMart"));
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registering email service
builder.Services.AddScoped<SmtpMailer>(_ =>
{
    var emailSettings = builder.Configuration.GetSection("SMTPService").Get<SmtpServiceConfiguration>();

    if (emailSettings is null)
    {
        throw new InvalidOperationException();
    }
    
    return new SmtpMailer(emailSettings.Url, emailSettings.Port, emailSettings.Email, emailSettings.Password)
        .Sender(emailSettings.Sender, emailSettings.Email);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();