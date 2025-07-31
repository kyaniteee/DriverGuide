using DriverGuide.Application;
using DriverGuide.Configurators;
using DriverGuide.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddJwt(builder.Configuration)
                .AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()//.WithOrigins("https://localhost:7181") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication(); // wa¿na jest kolejnoœæ - najpierw Authentication
app.UseAuthorization();  // potem Authorization

app.MapControllers();
app.Run();
