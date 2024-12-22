using JobTracker.Data;
using JobTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<JobTrackerContext>(
//    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<JobTrackerIdentityContext>(
//    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<JobTrackerContext>(
    options => options.UseInMemoryDatabase("AppDb"));
builder.Services.AddDbContext<JobTrackerIdentityContext>(
    options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<JobTrackerIdentityContext>();
builder.Services.AddAuthorization();

builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ToolService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
